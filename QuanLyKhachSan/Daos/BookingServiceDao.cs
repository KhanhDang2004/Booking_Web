using QuanLyKhachSan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Daos
{
    public class BookingServiceDao
    {
        DBQuanLyKhachSanEntities myDb = new DBQuanLyKhachSanEntities();
        
        public void Add(BookingService bookingService)
        {
            myDb.BookingServices.Add(bookingService);
            myDb.SaveChanges();
        }
    }
}