using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        Order_Restaurant_Db db = new Order_Restaurant_Db();
        // GET: Admin/User
        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            ViewBag.OrderTable = db.OrderTables.ToList();
            return View(db.Users.Where(x => x.Type == 0).OrderBy(n => n.ID).ToPagedList(page, pagesize));
        }

        public JsonResult ChangeStatus(int ID)
        {
            var cn = db.Users.Find(ID);
            if (cn.Status == true)
                cn.Status = false;
            else
                cn.Status = true;

            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }
    }
}