using QuanLyKhachSan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Daos
{
    public class ServiceDao
    {
        DBQuanLyKhachSanEntities myDb = new DBQuanLyKhachSanEntities();

        public List<Service> GetServices()
        {
            return myDb.Services.ToList();
        }

        public List<Service> GetServicesTop5()
        {
            return myDb.Services.Take(5).ToList();
        }

        public int GetCostById(int id)
        {
            return myDb.Services.FirstOrDefault(x => x.idService == id).cost;
        }

        public void add(Service service)
        {
            myDb.Services.Add(service);
            myDb.SaveChanges();
        }

        public void delete(int id)
        {
            var obj = myDb.Services.FirstOrDefault(x => x.idService == id);
            myDb.Services.Remove(obj);
            myDb.SaveChanges();
        }

        public void update(Service service)
        {
            var obj = myDb.Services.FirstOrDefault(x => x.idService == service.idService);
            obj.name = service.name;
            obj.cost = service.cost;
            myDb.SaveChanges();
        }

        public Service GetServiceID(int id)
        {
            return myDb.Services.FirstOrDefault(x => x.idService == id);
        }

        public List<BookingService> getCheck(int id)
        {
            return myDb.BookingServices.Where(x => x.idService == id).ToList();
        }

    }
}