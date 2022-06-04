using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebOrderTbRestaurant.Models;
using WebOrderTbRestaurant.Models.Business;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Controllers
{
    public class HomeController : Controller
    {
        private const string OrderSesstion = "ordersesstion";
        private const string BookFoodSesstion = "BookFood";
        Order_Restaurant_Db db = new Order_Restaurant_Db();
        // GET: Home
        public ActionResult Index(int page = 1, int pagesize = 12)
        {
            var sv = new FoodBusiness();
            ViewBag.Food_Slide = sv.ListSlideFood();

            ViewBag.Favorite = db.Favorites.ToList();

            //Danh sách món ăn
            var model = sv.PageListFood().ToPagedList(page, pagesize);

            //Bài viết
            ViewBag.TopNews = new NewsBusiness().getNews(1);
            ViewBag.BetweenNews = new NewsBusiness().getNews(4);
            ViewBag.ButtomNews = new NewsBusiness().getNews(1);
            ViewBag.NavigatorNews = new NewsBusiness().getNews(5);
            return View(model);
        }

        
        //Tìm kiếm món ăn
        public ActionResult Search(string keyword, string type = null, string order = null, int page = 1, int pagesize = 12)
        {
            //string[] key = keyword.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var lstfood = new List<Food>();
            if (type != null && order != null)
            {
                if (type == "name")
                {
                    if (order == "a-to-z")
                    {
                        lstfood = db.Foods.Where(x => x.Name.Contains(keyword)).OrderBy(x => x.Name).ToList();
                    }
                    else
                    {
                        lstfood = db.Foods.Where(x => x.Name.Contains(keyword)).OrderByDescending(x => x.Name).ToList();
                    }
                }
                else
                {
                    if (order == "desc")
                    {
                        lstfood = db.Foods.Where(x => x.Name.Contains(keyword)).OrderByDescending(x => x.Price).ToList();
                    }
                    else
                    {
                        lstfood = db.Foods.Where(x => x.Name.Contains(keyword)).OrderBy(x => x.Price).ToList();
                    }
                }

                ViewBag.Type = type;
                ViewBag.Order = order;
            }
            else
            {
                foreach (var item in db.Foods.ToList())
                {
                    if (item.Name == keyword)
                    {
                        lstfood.Insert(0, item);
                        new CookiesManage().SetFood_intoCookie(lstfood, true);
                    }
                    else if (item.Name.Contains(keyword))
                    {
                        lstfood.Add(item);
                        new CookiesManage().SetFood_intoCookie(lstfood, false);
                    }
                }
                //Thêm cookie
                new CookiesManage().SetFood_intoCookie(lstfood, false);
            }

            ViewBag.Favorite = db.Favorites.ToList();
            ViewBag.KeyWord = keyword;
            return View(lstfood.ToPagedList(page, pagesize));
        }

        //Thuộc tính autocomplete
        public JsonResult ListName(string q)
        {
            var data = new FoodBusiness().searchFood(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        //lấy giá trị ngày đặt bàn, giờ đặt bàn, số lượng khách
        [HttpPost]
        public ActionResult BookCustomer(BookCustomer entity)
        {

            Session[OrderSesstion] = entity;
            TempData["message"] = "Thêm thời gian đặt bàn thành công.";
            return Redirect("/dat-ban");
        }

        [ChildActionOnly]
        public PartialViewResult MainMenu()
        {
            var sv = new MenuBusiness();
            ViewBag.menu = sv.ListMenu();
            ViewBag.Food_Category = db.Food_Category.ToList();
            return PartialView();
        }

        public PartialViewResult Footer()
        {
            var sv = new MenuBusiness();
            ViewBag.menu = sv.ListMenu();
            return PartialView();
        }

        public ActionResult Contact(string type = null)
        {
            ViewBag.Type = type;
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}