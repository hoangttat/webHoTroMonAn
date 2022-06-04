using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOrderTbRestaurant.Models;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        Order_Restaurant_Db db = new Order_Restaurant_Db();
        // GET: Admin/Home
        public ActionResult Index()
        {
            //Tính tổng doanh thu
            TempData["TongDoanhThu"] = db.OrderTables.Where(n => n.Status == true).Sum(n => n.TotalMoney);

            //Đếm đơn đặt bàn mới
            TempData["DonDatMoi"] = db.OrderTables.Where(n => n.Status != true).Count();

            //Đếm số món ăn
            TempData["MonAn"] = db.Foods.Count();

            //Doanh thu hôm nay
            TempData["DoanhThuHomNay"] = db.OrderTables.Where(n => n.Status == true &&
                                                               DbFunctions.TruncateTime(n.CreatedDate) == DbFunctions.TruncateTime(DateTime.Now))
                                                               .Sum(x => x.TotalMoney);

            //Thống kê lượng món ăn đã đặt trước
            var lstproduct_id = db.Book_Food.Select(x => x.Food_ID).Distinct();
            var listProduct_sell = new List<BookFood>();
            foreach (var item in lstproduct_id)
            {
                var food = db.Foods.Find(item);
                var productsell = new BookFood();
                productsell.Food_Name = food.Name;
                productsell.Count = 0;
                productsell.TotalMoney = 0;
                foreach (var jtem in db.Book_Food.Where(x => x.Food_ID == item && x.OrderTable.Status == true))
                {
                    productsell.Count += (int)jtem.Count;
                    productsell.TotalMoney += jtem.Price;
                }
                listProduct_sell.Add(productsell);
            }
            ViewBag.product_sell = listProduct_sell.OrderByDescending(x => x.Count).Take(10).ToList();

            //Thống kê số bài viết
            ViewBag.BaiViet = db.News.Count();

            //Thống kê đơn đạt hàng hôm nay
            ViewBag.Order_today = db.OrderTables.Where(x => DbFunctions.TruncateTime(x.CreatedDate) == DbFunctions.TruncateTime(DateTime.Now)).Count();

            //Thống kê user đã đăng ký
            ViewBag.user = db.Users.Where(x => x.Type == 0).Count();

            //Đơn đăt đã thanh toán
            ViewBag.OrderPait = db.OrderTables.Where(x => x.Status == true).Count();

            //Tổng đơn hàng
            ViewBag.TongDH = db.OrderTables.Count();


            //Thống kê món ăn đã bán chạy trong tháng
            ViewBag.Month = DateTime.Now.Month;
            var lstsp_id = db.Book_Food.Where(x => x.OrderTable.CreatedDate.Value.Month == DateTime.Now.Month).Select(x => x.Food_ID).Distinct();
            var listsp_sell = new List<BookFood>();
            foreach (var item in lstsp_id)
            {
                var product = db.Foods.Find(item);
                var productsell = new BookFood();
                productsell.Food_Name = product.Name;
                productsell.Count = 0;
                productsell.TotalMoney = 0;
                foreach (var jtem in db.Book_Food.Where(x => x.Food_ID == item && x.OrderTable.CreatedDate.Value.Month == DateTime.Now.Month && x.OrderTable.Status == true))
                {
                    productsell.Count += (int)jtem.Count;
                    productsell.TotalMoney += jtem.Price * jtem.Count;
                }
                listsp_sell.Add(productsell);
            }
            ViewBag.sp_sell_month = listsp_sell.OrderByDescending(x => x.Count).Take(10).ToList();

            //Thống kê Sản phẩm đã bán theo danh mục
            var listSpByNSX_sell = new List<BookFood>();
            foreach (var item in db.Food_Category.ToList())
            {
                var pro = new BookFood();
                pro.Food_Category_Name = item.Name;
                pro.Count = 0;
                pro.TotalMoney = 0;
                foreach (var jtem in db.Book_Food.Where(x => x.Food.Food_CategoryID == item.ID && x.OrderTable.Status == true))
                {
                    pro.Count += (int)jtem.Count;
                    pro.TotalMoney += jtem.Price * jtem.Count;
                }
                listSpByNSX_sell.Add(pro);
            }
            ViewBag.sp_sell_nsx = listSpByNSX_sell.OrderByDescending(x => x.Count).Take(10).ToList();
            return View();
        }

        public ActionResult TotalSale_Month()
        {
            int[] month = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var lstTotal = new List<ChartDTO>();
            for (int i = 1; i <= 12; i++)
            {
                var model = db.OrderTables.Where(x => x.CreatedDate.Value.Month == i).Count();

                var totalsale = new ChartDTO();
                totalsale.thang = i;
                totalsale.tong = model;
              
                lstTotal.Add(totalsale);
            }
            return Json(lstTotal, JsonRequestBehavior.AllowGet);

        }

        public ActionResult TotalSale_Brand()
        {
            var lstTotal = new List<ChartDTO>();
            var lstNSX = db.Food_Category.ToList();
            foreach (var item in lstNSX)
            {
                var model = (from ct in db.Book_Food
                             join dh in db.OrderTables on ct.OrderTable_ID equals dh.ID
                             where ct.Food.Food_CategoryID == item.ID && dh.Status == true
                             select new ChartDTO
                             {
                                 tong = dh.TotalMoney
                             }).Sum(x => x.tong);
                var totalsale = new ChartDTO();
                totalsale.Food_Category_Name = item.Name;
                if (model != null)
                    totalsale.tong = model;
                else
                    totalsale.tong = 0;
                lstTotal.Add(totalsale);
            }
            return Json(lstTotal, JsonRequestBehavior.AllowGet);

        }
    }
}