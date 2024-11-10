using QuanLyKhachSan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Daos
{
    public class RoomCommentDao
    {
        DBQuanLyKhachSanEntities myDb = new DBQuanLyKhachSanEntities();
        public void Add(RoomComment roomComment)
        {
            myDb.RoomComments.Add(roomComment);
            myDb.SaveChanges();
        }

        public List<RoomComment> GetByIdRoom(int idRoom)
        {
            return myDb.RoomComments.Where(x => x.idRoom == idRoom).OrderByDescending(x => x.createdDate).ToList();
        }

        public double getAve(int idRoom)
        {
            var qr =  myDb.RoomComments.Where(x => x.idRoom == idRoom).ToList();
            return qr.Any() ? qr.Average(x => x.star) : 5;
        }
    }
}
