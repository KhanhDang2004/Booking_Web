﻿using QuanLyKhachSan.Daos;
using QuanLyKhachSan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyKhachSan.Controllers.Public
{
    public class PublicAuthenticationController : Controller
    {
        DBQuanLyKhachSanEntities myDb = new DBQuanLyKhachSanEntities();
        // GET: PublicAuthentication
        UserDao userDao = new UserDao();
        // GET: AdminAuthentication
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {

            return View();
        }
        public ActionResult Register()
        {

            return View();
        }
        public ActionResult CheckOTP()
        {

            return View();
        }

        public ActionResult ForgotPassword()
        {

            return View();
        }

        public ActionResult ResetPassword(int id)
        {
            ViewBag.IdUser = id;
            return View();
        }

        [HttpPost]
        public ActionResult UpdatePassword(FormCollection form)
        {
            string passwordNew = form["password"];
            string rePasswordNew = form["rePassword"];
            int id = Int32.Parse(form["idUser"]);
            if (passwordNew.Equals(rePasswordNew))
            {
                User user = myDb.Users.FirstOrDefault(x => x.idUser == id);
                user.password = userDao.md5(passwordNew);
                myDb.SaveChanges();
                ViewBag.mess = "ResetOk";
                return View("Login");
            }
            else
            {
                ViewBag.mess = "Error";
                ViewBag.IdUser = id;
                return View("ResetPassword");
            }

        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            User user = new User()
            {
                userName = form["userName"],
                password = form["password"]
            };
            string passwordMd5 = userDao.md5(form["password"]);
            bool checkLogin = userDao.checkLogin(user.userName, passwordMd5);
            if (checkLogin)
            {
                var userInformation = userDao.getUserByUserName(user.userName);
                if(userInformation.idRole == 3)
                {
                    Session.Add("USER", userInformation);
                    return RedirectToAction("Index", "PublicHome");
                }
                else
                {
                    Session.Add("ADMIN", userInformation);
                    return RedirectToAction("Index", "AdminHome");
                }   
            }
            else
            {
                ViewBag.mess = "Error";
                return View("Login");
            }

        }

        [HttpPost]
        public ActionResult VerifyOTP(FormCollection form)
        {

            var otp = form["otp"];
            var otpcheck = (string)Session["Otp"];
            if (otp.Equals(otpcheck))
            {
                var user = (User)Session["RegisterUser"];
                userDao.add(user);
                ViewBag.mess = "Success";
                return View("Login");
            }
            ViewBag.mess = "Error";
            return View("CheckOTP");
        }

        [HttpPost]
        public ActionResult RePassword(FormCollection form)
        {

            var email = form["email"];
            var check = userDao.getUserByEmail(email);
            if (check != null)
            {
                var idUser = check.idUser;
                string html = "Vui lòng nhấn vào link để reset mật khẩu : <a href='https://localhost:44385/PublicAuthentication/ResetPassword/" + idUser + "'>Tại đây</a>" ;
                sendMail(check.email, html);
                ViewBag.mess = "Success";
                return View("ForgotPassword");
            }
            ViewBag.mess = "Error";
            return View("ForgotPassword");
        }

        [HttpPost]
        public ActionResult Register(User user,FormCollection form)
        {
            string rePassword = form["rePassword"];
            bool checkExistUserName = userDao.checkExistUsername(user.userName);
            if (checkExistUserName)
            {
                ViewBag.mess = "ErrorExist";
                return View("Login");
            } else
            {
                if (!user.password.Equals(rePassword))
                {
                    ViewBag.mess = "ErrorPassword";
                    return View("Login");
                }
                else
                {
                    user.password = userDao.md5(user.password);
                    user.idRole = 3;
                    var otp = RandomNumber(6);
                    Session.Add("RegisterUser", user);
                    Session.Add("Otp", otp);
                    string html = "Mã xác thực OTP đăng ký của bạn là :  " + otp;
                    sendMail(user.email, html);

                    ViewBag.mess = "Success";
                    return View("CheckOTP");
                    
                }            
            }
        }
        public ActionResult Logout()
        {
            Session.Remove("User");
            return Redirect("/PublicHome/Index");
        }

        //random chuỗi số bất kỳ
        public static string RandomNumber(int numberRD)
        {
            string randomStr = "";
            try
            {

                int[] myIntArray = new int[numberRD];
                int x;
                //that is to create the random # and add it to uour string
                Random autoRand = new Random();
                for (x = 0; x < numberRD; x++)
                {
                    myIntArray[x] = System.Convert.ToInt32(autoRand.Next(0, 9));
                    randomStr += (myIntArray[x].ToString());
                }
            }
            catch (Exception ex)
            {
                randomStr = "error";
            }
            return randomStr;
        }

        public void sendMail(string email, string body)
        {
            try
            {
                var formEmailAddress = ConfigurationManager.AppSettings["FormEmailAddress"];
                var formEmailDisplayName = ConfigurationManager.AppSettings["FormEmailDisplayName"];
                var formEmailPassword = ConfigurationManager.AppSettings["FormEmailPassword"];
                var smtpHost = ConfigurationManager.AppSettings["SMTPHost"];
                var smtpPort = ConfigurationManager.AppSettings["SMTPPort"];
                var enableSsl = ConfigurationManager.AppSettings["EnabledSSL"];

                // Kiểm tra null
                if (string.IsNullOrEmpty(formEmailAddress) ||
                    string.IsNullOrEmpty(formEmailDisplayName) ||
                    string.IsNullOrEmpty(formEmailPassword) ||
                    string.IsNullOrEmpty(smtpHost) ||
                    string.IsNullOrEmpty(smtpPort) ||
                    string.IsNullOrEmpty(enableSsl))
                {
                    throw new Exception("Thiếu cấu hình trong Web.config");
                }

                MailMessage message = new MailMessage(
                    new MailAddress(formEmailAddress, formEmailDisplayName),
                    new MailAddress(email)
                );
                message.Subject = "Thông báo";
                message.IsBodyHtml = true;
                message.Body = body;

                var client = new SmtpClient
                {
                    Credentials = new NetworkCredential(formEmailAddress, formEmailPassword),
                    Host = smtpHost,
                    EnableSsl = bool.Parse(enableSsl),
                    Port = Convert.ToInt32(smtpPort)
                };

                client.Send(message);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi hoặc xử lý ngoại lệ
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }

    }
}