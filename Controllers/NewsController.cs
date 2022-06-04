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
    public class NewsController : Controller
    {
        Order_Restaurant_Db db = new Order_Restaurant_Db();
        // GET: News
        public ActionResult Detail(string Metatitle, long ID)
        {
            ViewBag.NavigatorNews = new NewsBusiness().getNews(7);
            
            var news = db.News.Find(ID);
            ViewBag.News = news;

            //Lưu bài viết đã đọc
           
            if(Session["RecentNews"] != null)
            {
                var recentnews = Session["RecentNews"] as List<News>;
                if(!recentnews.Exists(x => x.ID == ID))
                {
                    recentnews.Add(news);
                    Session["RecentNews"] = recentnews;
                }
            }
            else
            {
                var recentnews = new List<News>();
                recentnews.Add(news);
                Session["RecentNews"] = recentnews;
            }
            return View();
        }

        public ActionResult Index(int page = 1, int pagesize = 12)
        {
            var model = db.News.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pagesize);
            return View(model);
        }
    }
}