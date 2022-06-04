using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOrderTbRestaurant.Models.Business;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Controllers
{
    public class FoodController : Controller
    {
        Order_Restaurant_Db db = new Order_Restaurant_Db();
        // GET: Food
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(long id)
        {
            var food = db.Foods.Find(id);
            ViewBag.foodDetail = food;

            //thêm cookei
            var lstfood = new List<Food>();
            lstfood.Insert(0, food);
            new CookiesManage().SetFood_intoCookie(lstfood, true);

            //Món ăn cùng danh mục
            ViewBag.lstFoodSameCategory = db.Foods.Where(x => x.Food_CategoryID == food.Food_CategoryID).ToList();

            //Bài viết
            ViewBag.lstNews = db.News.OrderByDescending(x => x.View).ToList();
            return View();

        }

        public JsonResult AddFavorite(long User_ID, long Food_ID, bool isLike)
        {
            if (isLike)
            {
                //thêm cookei
                var lstfood = new List<Food>();
                var food = db.Foods.Find(Food_ID);
                lstfood.Insert(0, food);
                new CookiesManage().SetFood_intoCookie(lstfood, true);

                var fav = new Favorite();
                fav.Food_ID = Food_ID;
                fav.User_ID = User_ID;
                db.Favorites.Add(fav);
            }
            else
            {
                var fav = db.Favorites.FirstOrDefault(x => x.User_ID == User_ID && x.Food_ID == Food_ID);
                db.Favorites.Remove(fav);
            }

            db.SaveChanges();
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Menu(string type = null, string order = null, int page = 1, int pagesize = 12)
        {
            if (type != null && order != null)
            {
                var food = new List<Food>();
                if (type == "name")
                {
                    if (order == "a-to-z")
                    {
                        food = db.Foods.OrderBy(x => x.Name).ToList();
                    }
                    else
                    {
                        food = db.Foods.OrderByDescending(x => x.Name).ToList();
                    }
                }
                else
                {
                    if (order == "desc")
                    {
                        food = db.Foods.OrderByDescending(x => x.Price).ToList();
                    }
                    else
                    {
                        food = db.Foods.OrderBy(x => x.Price).ToList();
                    }
                }

                ViewBag.Type = type;
                ViewBag.Order = order;
                ViewBag.Favorite = db.Favorites.ToList();
                return View(food.ToPagedList(page, pagesize));
            }
            else
            {
                var model = db.Foods.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pagesize);
                ViewBag.Favorite = db.Favorites.ToList();
                return View(model);
            }
            
        }

        public ActionResult FoodCategory(string Link, long ID, string type = null, string order = null, int page = 1, int pagesize = 12)
        {
            ViewBag.FoodCategory = db.Food_Category.Find(ID);
            
            if (type != null && order != null)
            {
                var food = new List<Food>();
                if(type == "name")
                {
                    if(order == "a-to-z")
                    {
                        food = db.Foods.Where(x => x.Food_CategoryID == ID).OrderBy(x => x.Name).ToList();
                    }
                    else
                    {
                        food = db.Foods.Where(x => x.Food_CategoryID == ID).OrderByDescending(x => x.Name).ToList();
                    }
                }
                else
                {
                    if (order == "desc")
                    {
                        food = db.Foods.Where(x => x.Food_CategoryID == ID).OrderByDescending(x => x.Price).ToList();
                    }
                    else
                    {
                        food = db.Foods.Where(x => x.Food_CategoryID == ID).OrderBy(x => x.Price).ToList();
                    }
                }

                ViewBag.Type = type;
                ViewBag.Order = order;
                ViewBag.Favorite = db.Favorites.ToList();
                return View(food.ToPagedList(page, pagesize));
            }
            else
            {
                var food = db.Foods.Where(x => x.Food_CategoryID == ID).ToList();
                //Thêm cookei
                new CookiesManage().SetFood_intoCookie(food, false);

                var model = food.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pagesize);
                ViewBag.Favorite = db.Favorites.ToList();
                return View(model);
            }
            
        }

    }
}