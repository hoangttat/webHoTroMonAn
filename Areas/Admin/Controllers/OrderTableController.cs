using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOrderTbRestaurant.Areas.Admin.Models;
using WebOrderTbRestaurant.Models.Business;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Areas.Admin.Controllers
{
    public class OrderTableController : Controller
    {
        Order_Restaurant_Db db = new Order_Restaurant_Db();
        // GET: Admin/Order
        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            var or = new OrderBusiness();
            var model = or.ListAll().ToPagedList(page, pagesize); //Phân trang 
            var order = or.ListAll();
            var list = new List<long>();
            var bookfood = new BookMenuBusiness();
            foreach (var item in order)
            {
                if (bookfood.CheckID(item.ID))
                {
                    list.Add(item.ID);//Lấy thực đơn của mỗi khách hàng
                }

            }
            ViewBag.IDorder = list;

            return View(model);
        }

        public JsonResult ChangeStatus(long ID)
        {
            var order = db.OrderTables.Find(ID);
            order.Status = true;

            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }

        public List<TimeBook> GetAllTime()
        {
            var list = new List<TimeBook>
            {
                   new TimeBook {id = 1 , time = "0:00 AM" },
                   new TimeBook {id = 2 , time = "0:30 AM" },
                   new TimeBook {id = 3 , time = "1:00 AM" },
                   new TimeBook {id = 4 , time = "1:30 AM" },
                   new TimeBook {id = 5 , time = "2:00 AM" },
                   new TimeBook {id = 6 , time = "2:30 AM" },
                   new TimeBook {id = 8 , time = "3:00 AM" },
                   new TimeBook {id = 9 , time = "3:30 AM" },
                   new TimeBook {id = 10 , time = "4:00 AM" },
                   new TimeBook {id = 11 , time = "4:30 AM" },
                   new TimeBook {id = 12 , time = "5:00 AM" },
                   new TimeBook {id = 13 , time = "5:30 AM" },
                   new TimeBook {id = 14 , time = "6:00 AM" },
                   new TimeBook {id = 15 , time = "6:30 AM" },
                   new TimeBook {id = 16 , time = "7:00 AM" },
                   new TimeBook {id = 17 , time = "7:30 AM" },
                   new TimeBook {id = 18 , time = "8:00 AM" },
                   new TimeBook {id = 19 , time = "8:30 AM" },
                   new TimeBook {id = 20 , time = "9:00 AM" },
                   new TimeBook {id = 21 , time = "9:30 AM" },
                   new TimeBook {id = 22 , time = "10:00 AM" },
                   new TimeBook {id = 23 , time = "10:30 AM" },
                   new TimeBook {id = 24 , time = "11:00 AM" },
                   new TimeBook {id = 25 , time = "11:30 AM" },
                   new TimeBook {id = 26 , time = "12:00 AM" },
                   new TimeBook {id = 27 , time = "12:30 AM" },
                   new TimeBook {id = 28 , time = "13:00 AM" },
                   new TimeBook {id = 29 , time = "13:30 AM" },
                   new TimeBook {id = 30 , time = "14:00 AM" },
                   new TimeBook {id = 31 , time = "14:30 AM" },
                   new TimeBook {id = 32 , time = "15:00 AM" },
                   new TimeBook {id = 33 , time = "15:30 AM" },
                   new TimeBook {id = 34 , time = "16:00 AM" },
                   new TimeBook {id = 35 , time = "16:30 AM" },
                   new TimeBook {id = 36 , time = "17:00 AM" },
                   new TimeBook {id = 37 , time = "17:30 AM" },
                   new TimeBook {id = 38 , time = "18:00 AM" },
                   new TimeBook {id = 39 , time = "18:30 AM" },
                   new TimeBook {id = 40 , time = "19:00 AM" },
                   new TimeBook {id = 41 , time = "19:30 AM" },
                   new TimeBook {id = 42 , time = "20:00 AM" },
                   new TimeBook {id = 43 , time = "20:30 AM" },
                   new TimeBook {id = 44 , time = "21:00 AM" },
                   new TimeBook {id = 45 , time = "21:30 AM" },
                   new TimeBook {id = 46 , time = "22:00 AM" },
                   new TimeBook {id = 47 , time = "22:30 AM" },
                   new TimeBook {id = 48 , time = "23:00 AM" },
                   new TimeBook {id = 49 , time = "23:30 AM" }
            };
            return list.ToList();
        }



        // GET: Admin/Order/Edit/5
        public ActionResult Edit(long id)
        {
            ViewBag.Order = new OrderBusiness().FindID(id);
            ViewBag.selectTime = GetAllTime();
            return View();
        }

        //Lấy ra thông tin dropdownList
        public void SetViewBag(long? selectTime = null)
        {
            var dao = new TimeBook();
            ViewBag.CategoryID = new SelectList(GetAllTime(), "time", "time", selectTime);
        }

        // POST: Admin/Order/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(OrderTable orTB)
        {
            ViewBag.selectTime = new SelectList(GetAllTime(), "time", "time");
            var res = new OrderBusiness().EditOrder(orTB);
            if (res)
            {
                TempData["message"] = "Cập nhật thông tin khách hàng thành công";
                return RedirectToAction("Index", "OrderTable");
            }
            else
            {
                TempData["message"] = "Cập nhật thông tin khách hàng thành công";
                return View();
            }

        }

        // GET: Admin/Order/Delete/5
        [HttpPost]
        public JsonResult Delete(long id)
        {
            var order = new OrderBusiness().DeleteOrder(id);
            if (order)
            {
                SetAlert("Xóa khách hàng thành công", "success");
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                SetAlert("Xóa Không thành công", "warning");
                return Json(new
                {
                    status = false
                });
            }

        }




        public void SetAlert(string message, string type)
        {
            //Giống ViewBag
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}