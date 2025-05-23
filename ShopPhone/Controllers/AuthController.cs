﻿using ShopNoiThat.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using ShopNoiThat.Library;
using System.Text.RegularExpressions;
using System.Web.Mvc;
namespace ShopNoiThat.Controllers
{

    public class AuthController : Controller
    {
        ShopNoiThatDbContext db = new ShopNoiThatDbContext();
        // lấy giá trị r mã hoá 
        // dò vs database 
        public string GenerateOTP()
        {
            Random rand = new Random();
            return rand.Next(100000, 999999).ToString(); // 6 số
        }

        public void login(FormCollection fc)
        {

            string recaptchaResponse = Request["g-recaptcha-response"];
            if (string.IsNullOrWhiteSpace(recaptchaResponse) || !IsCaptchaValid(recaptchaResponse))
            {
                Message.set_flash("Vui lòng xác nhận bạn không phải robot", "error");
                if (!Response.IsRequestBeingRedirected)
                    Response.Redirect("~/");
                return;
            }

            string Username = fc["uname"];
            string Pass = Mystring.ToMD5(fc["psw"]);

            var user = db.users.FirstOrDefault(m => m.username == Username && m.access == 1);

            if (user == null)
            {
                Message.set_flash("Tên đăng nhập không tồn tại", "error");
            }
            else
            {
                if (user.locked_until != null && user.locked_until > DateTime.Now)
                {
                    // Nếu tài khoản bị khóa, kiểm tra OTP
                    string enteredOtp = fc["otp"];  // Nhận OTP từ form

                    if (string.IsNullOrEmpty(enteredOtp))
                    {
                        Message.set_flash("Tài khoản bị khóa. Vui lòng nhập mã OTP để mở khóa.", "error");
                        Response.Redirect("~/login"); // thêm dòng này
                    }
                    else if (user.otp_code == enteredOtp && user.otp_created_at != null && user.otp_created_at.Value.AddMinutes(5) > DateTime.Now)
                    {
                        // Kiểm tra OTP và thời gian hợp lệ
                        user.failed_login_count = 0;
                        user.locked_until = null;
                        user.otp_code = null;
                        user.otp_created_at = null;

                        db.SaveChanges();
                        Message.set_flash("Tài khoản đã được mở khóa. Bạn có thể đăng nhập.", "success");
                    }
                    else
                    {
                        Message.set_flash("Mã OTP không hợp lệ hoặc đã hết hạn.", "error");
                    }
                }
                else
                {
                    if (user.password != Pass || user.status != 1)
                    {
                        user.failed_login_count = (user.failed_login_count ?? 0) + 1;
                        user.last_failed_login = DateTime.Now;

                        if (user.failed_login_count >= 5)
                        {
                            user.locked_until = DateTime.Now.AddMinutes(5);
                            string otp = GenerateOTP();
                            user.otp_code = otp;
                            user.otp_created_at = DateTime.Now;
                            SendOTP(user.email, otp);

                            Message.set_flash("Nhập sai quá 5 lần. Tài khoản tạm khóa 5 phút. Vui lòng kiểm tra email để lấy mã OTP.", "error");
                        }
                        else
                        {
                            Message.set_flash("Mật khẩu không đúng", "error");
                        }

                        db.SaveChanges();
                    }
                    else
                    {
                        user.failed_login_count = 0;
                        user.locked_until = null;
                        user.otp_code = null;
                        user.otp_created_at = null;
                        db.SaveChanges();

                        Session["id"] = user.ID;
                        Session["user"] = user.username;
                        ViewBag.name = Session["user"];

                        Message.set_flash("Đăng nhập thành công", "success");
                        Response.Redirect("~/");
                    }
                }
            }

            if (!Response.IsRequestBeingRedirected)
                Response.Redirect("~/");
        }


        public void logout()
        {
            Session["id"] = "";
            Session["user"] = "";
            Response.Redirect("~/");
            Message.set_flash("Đăng xuất thành công", "success");
        }

        public void register(Muser muser, FormCollection fc)
        {
            string uname = fc["uname"];
            string fname = fc["fname"];
            string rawPass = fc["psw"];  // mật khẩu gốc chưa băm
            string email = fc["email"];
            string phone = fc["phone"];

            // Kiểm tra mật khẩu mạnh (>= 8 ký tự, có chữ hoa, chữ thường, số, ký tự đặc biệt)
            var passPattern = new Regex(@"^(?=.{8,}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).*$");
            if (!passPattern.IsMatch(rawPass))
            {
                Message.set_flash("Mật khẩu phải chứa ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.", "error");
                Response.Redirect("~/");
                return;
            }

            // Chuyển mật khẩu sang MD5
            string Pass = Mystring.ToMD5(rawPass);

            if (ModelState.IsValid)
            {
                var Luser = db.users.Where(m => m.status == 1 && m.username == uname && m.access == 1);
                if (Luser.Count() > 0)
                {
                    Message.set_flash("Tên Đăng nhập đã tồn tại", "error");
                    Response.Redirect("~/");
                }
                else
                {
                    muser.img = "defalt.png";
                    muser.password = Pass;
                    muser.username = uname;
                    muser.fullname = fname;
                    muser.email = email;
                    muser.phone = phone;
                    muser.gender = "nam";  // Bạn có thể thay đổi theo nhu cầu
                    muser.access = 1;
                    muser.created_at = DateTime.Now;
                    muser.updated_at = DateTime.Now;
                    muser.created_by = 1;
                    muser.updated_by = 1;
                    muser.status = 1;

                    // Thêm user vào cơ sở dữ liệu
                    db.users.Add(muser);
                    db.SaveChanges();

                    Message.set_flash("Tạo user thành công", "success");
                    Response.Redirect("~/");
                }
            }
        }
        // quên pass chuyển trang kp
        public ActionResult forgetpass()
        {
            return View();
        }
        // khôi phục 
        public ActionResult newPasswordFG(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // k hợp lệ
            }
            Muser muser = db.users.Find(id);
            if (muser == null)
            {
                return HttpNotFound(); // không tồn tại
            }
            return View("_newPasswordFG", muser); // nhập mk new kp
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*
         Mã này liên quan đến chức năng thay đổi mật khẩu cho người dùng đã quên mật khẩu.

Phương thức này được khai báo là một phương thức bất đồng bộ, có kiểu trả về là ActionResult.
Nó nhận đối số là đối tượng Muser và đối tượng FormCollection để lấy dữ liệu mật khẩu mới và xác nhận mật khẩu mới.
Tiếp theo, phương thức thực hiện kiểm tra nếu mật khẩu mới không trùng khớp với mật khẩu xác nhận thì thông báo lỗi và trả về lại view "_newPasswordFG" với đối tượng Muser ban đầu để người dùng nhập lại thông tin.
Nếu mật khẩu mới trùng khớp với mật khẩu xác nhận thì phương thức thực hiện kiểm tra ModelState.IsValid để xác định đối tượng Muser đã được xác thực hợp lệ hay chưa.
Nếu đối tượng Muser hợp lệ, phương thức sử dụng phương thức Find của đối tượng db để tìm kiếm người dùng với ID được truyền vào.
Sau đó, phương thức cập nhật thông tin của người dùng bằng các giá trị từ đối tượng Muser truyền vào và cập nhật mật khẩu mới đã mã hóa MD5.
Cuối cùng, phương thức lưu lại thay đổi vào cơ sở dữ liệu và hiển thị thông báo thành công, sau đó chuyển hướng người dùng về trang chủ.
Nếu đối tượng Muser không hợp lệ, phương thức trả về view "_newPasswordFG" với đối tượng Muser ban đầu để người dùng nhập lại thông tin và thông báo lỗi.
         */
        public async Task<ActionResult> newPasswordFG(Muser muser, FormCollection fc)
        {
            string rePass = Mystring.ToMD5(fc["rePass"]);
            string newPass = Mystring.ToMD5(fc["password1"]);
            if (rePass != newPass)
            {
                ViewBag.status = "2 Mật khẩu không khớp";
                return View("_newPasswordFG", muser);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var updatedPass = db.users.Find(muser.ID);
                    updatedPass.fullname = muser.fullname;
                    updatedPass.username = muser.username;
                    updatedPass.email = muser.email;
                    updatedPass.phone = muser.phone;
                    updatedPass.gender = muser.gender;
                    updatedPass.img = "bav";
                    updatedPass.password = newPass;
                    updatedPass.access = 1;
                    updatedPass.created_at = muser.created_at;
                    updatedPass.updated_at = DateTime.Now;
                    updatedPass.created_by = muser.created_by;
                    updatedPass.updated_by = muser.ID;
                    updatedPass.status = 1;
                    db.users.Attach(updatedPass);
                    db.Entry(updatedPass).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    Message.set_flash("Reset Mật Khẩu thành công", "success");
                    return Redirect("~/");
                }
            }
            ViewBag.status = "Vui lòng thử lại";
            return View("_newPasswordFG", muser);
        }
        public ActionResult sendMail()
        {
            //var username = Request.QueryString["username"];
            ViewBag.mess = "";
            var username = Request.Form["username"];
            var list = db.users.Where(m => m.access == 1 && m.status == 1 && m.username == username).Count();
            if (list <= 0)
            {
                ViewBag.mess = "Tên Đăng Nhập Không Đúng";
                return View("forgetPass");
            }
            else
            {
                var item = db.users.Where(m => m.access == 1 && m.status == 1 && m.username == username).First();
                // email gửi đi và email nhận
                MailMessage mm = new MailMessage(Util.email, item.email);
                mm.Subject = "Cấp Lại Mật khẩu Shop noi that";
                mm.Body = "Dear Mr." + item.fullname + "," +
                    "Chúng tôi đã nhận được yêu cầu reset đổi mật khẩu của bạn, vui lòng tạo mới mật khẩu qua đường dẫn sau : http://localhost:22222/newPass/" + item.ID + "";
                mm.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                /// Email dùng để gửi đi
                NetworkCredential nc = new NetworkCredential(Util.email, Util.password);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = nc;
                smtp.Send(mm);
                ViewBag.mess = item.email;
                return View("sendMailFinish");
            }
        }
        public void SendOTP(string toEmail, string otp)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(toEmail);
            mail.From = new MailAddress("2224802010123@student.tdmu.edu.vn"); // Gmail của bạn
            mail.Subject = "Mã OTP xác minh tài khoản";
            mail.Body = $"Mã OTP của bạn là: {otp}. Mã có hiệu lực trong 5 phút.";

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("2224802010123@student.tdmu.edu.vn", "zwpg ozbj acnd eula");

            smtp.Send(mail);
        }

        private bool IsCaptchaValid(string response)
        {
            // Thay bằng Secret Key của bạn (không để xuống dòng)
            var secret = "6LfoCywrAAAAAP5pQuyrBV1ZvQ3OpfS5UgJ2Ags-\r\n";
            using (var client = new WebClient())
            {
                var result = client.DownloadString(
                    $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={response}"
                );
                var obj = Newtonsoft.Json.Linq.JObject.Parse(result);
                return (bool)obj["success"];
            }
        }
    }
}