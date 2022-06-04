using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        Order_Restaurant_Db db = new Order_Restaurant_Db();
        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            var model = db.Users.Where(x => x.Type == 1 && x.Account.Trim() != "admin").OrderBy(x => x.ID).ToPagedList(page, pagesize);
            return View(model);
        }

        public JsonResult ChangeStatus(long ID)
        {
            var user = db.Users.Find(ID);
            if (user.Status == false)
                user.Status = true;
            else
                user.Status = false;

            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Delete(int ID)
        {

            try
            {
                var cn = db.Users.Find(ID);
                db.Users.Remove(cn);
                db.SaveChanges();
                return Json(new
                {
                    status = true
                });
            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }

        }


        [HttpPost]
        public ActionResult Add(User entity)
        {
            try
            {
                entity.Type = 1;
                entity.Status = true;
                db.Users.Add(entity);
                db.SaveChanges();
                TempData["message"] = "Thêm truy cập thành công";
                return RedirectToAction("Index");

            }
            catch
            {
                TempData["message"] = "Thêm truy cập KHÔNG thành công";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(User entity)
        {
            try
            {
                var cc = db.Users.Find(entity.ID);
                cc.Full_Name = entity.Full_Name;
                cc.Phone = entity.Phone;
                cc.Email = entity.Email;
                cc.Address = entity.Address;
                db.SaveChanges();
                TempData["message"] = "Cập nhật truy cập thành công";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "Cập nhật truy cập KHÔNG thành công";
                return RedirectToAction("Index");
            }
        }

        public JsonResult GetByID(int ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cn = db.Users.Find(ID);
            return Json(cn, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session["admin"] = null;
            return Redirect("/user/login");
        }

        public ActionResult ChangePass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult frmChangePass(string Old_Pass, string New_Pass)
        {
            var user = Session["admin"] as User;
            if (user.Password.Trim() == Old_Pass)
            {
                var users = db.Users.Find(user.ID);
                users.Password = New_Pass;
                db.SaveChanges();

                Session["admin"] = null;
                TempData["message"] = "Đổi mật khẩu thành công. Bạn vui lòng đăng nhập lại.";
                return Redirect("/user/login");
            }
            else
            {
                TempData["message"] = "Mật khẩu cũ không đúng. Vui lòng nhập lại";
                return RedirectToAction("changePass");
            }


        }
    }
}