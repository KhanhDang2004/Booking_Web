using QuanLyKhachSan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Daos
{
    public class TypeDao
    {
        DBQuanLyKhachSanEntities myDb = new DBQuanLyKhachSanEntities();

        public List<QuanLyKhachSan.Models.Type> GetTypes()
        {
            return myDb.Types.ToList();
        }

        public void add(QuanLyKhachSan.Models.Type type)
        {
            myDb.Types.Add(type);
            myDb.SaveChanges();
        }

        public void delete(int id)
        {
              var obj = myDb.Types.FirstOrDefault(x => x.idType == id);
              myDb.Types.Remove(obj);
              myDb.SaveChanges();
        }

        public void update(QuanLyKhachSan.Models.Type type)
        {
            var obj = myDb.Types.FirstOrDefault(x => x.idType == type.idType);
            obj.name = type.name;
            myDb.SaveChanges();
        }

        public QuanLyKhachSan.Models.Type getTypeId(int id)
        {
            return myDb.Types.FirstOrDefault(x => x.idType == id);
        }

        public List<Room> getRoomType (int id)
        {
            return myDb.Rooms.Where(x => x.idType == id).ToList();
        }
    }
}