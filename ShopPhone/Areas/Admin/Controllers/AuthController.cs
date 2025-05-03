using ShopNoiThat.Common;
using ShopNoiThat.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShopNoiThat.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        // GET: Admin/Auth
        ShopNoiThatDbContext db = new ShopNoiThatDbContext();
        public ActionResult login()
        {
            return View("_login");
        }
        [HttpPost]
        public ActionResult login(FormCollection fc)
        {
            string Username = fc["username"];
            string Pass = Mystring.ToMD5(fc["password"]);

            // Lấy người dùng đúng username và status = 1
            var user_account = db.users.SingleOrDefault(m => m.status == 1 && m.username == Username);

            if (user_account == null)
            {
                ViewBag.error = "Tên đăng nhập không đúng hoặc tài khoản đã bị khóa.";
            }
            else if (user_account.password != Pass)
            {
                ViewBag.error = "Mật khẩu không đúng.";
            }
            else
            {
                // Đăng nhập thành công
                role role = db.roles.FirstOrDefault(m => m.parentId == user_account.access);
                var userSession = new Userlogin
                {
                    UserName = user_account.username,
                    UserID = user_account.ID,
                    GroupID = role?.GropID,
                    AccessName = role?.accessName
                };

                Session.Add(CommonConstants.USER_SESSION, userSession);
                Session["Admin_id"] = user_account.ID;
                Session["Admin_user"] = user_account.username;
                Session["Admin_fullname"] = user_account.fullname;

                return Redirect("~/Admin");
            }

            return View("_login");
        }

        // khi user đăng xuất
        public ActionResult logout()
        {
            Session["Admin_id"] = "";
            Session["Admin_user"] = "";
            Response.Redirect("~/Admin");
            Session.Clear(); // xóa toàn bộ session
            TempData["logoutMessage"] = "Phiên đăng nhập đã hết hạn hoặc bạn đã đăng xuất.";
            return RedirectToAction("login", "Auth");
            
        }
 
        // GET: Admin/User/Edit/5
        //chỉnh sửa ng dùng
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Muser muser = db.users.Find(id);
            ViewBag.role = db.roles.Where(m => m.parentId == muser.access).First();
            if (muser == null)
            {
                return HttpNotFound();
            }
            return View("_information", muser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //cập nhật thông tin ng dùng
        public ActionResult Edit(Muser muser)
        {
            if (ModelState.IsValid)
            {
                muser.img = "ádasd";
                muser.access = 0;
                muser.created_at = DateTime.Now;
                muser.updated_at = DateTime.Now;
                muser.created_by = int.Parse(Session["Admin_id"].ToString());
                muser.updated_by = int.Parse(Session["Admin_id"].ToString());
                db.Entry(muser).State = EntityState.Modified;
                db.SaveChanges();
                Message.set_flash("Cập nhật thành công", "success");
                ViewBag.role = db.roles.Where(m => m.parentId == muser.access).First();
                return View("_information", muser);
            }
            Message.set_flash("Cập nhật Thất Bại", "danger");
            ViewBag.role = db.roles.Where(m => m.parentId == muser.access).First();
            return View("_information", muser);
        }

    }
}