using QuanLyKhachSan.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKhachSan.Controllers.Public
{
    public class KhachSanAPIController : Controller
    {
        // GET: KhachSanAPI
        public JsonResult Index()
        {
            DBQuanLyKhachSanEntities myDb = new DBQuanLyKhachSanEntities();
            ArrayList a = myDb.get("exec GetRooms"); 
            return Json(a,JsonRequestBehavior.AllowGet);
        }
    }
}