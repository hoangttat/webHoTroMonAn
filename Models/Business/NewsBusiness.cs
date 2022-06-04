using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Models.Business
{
    public class NewsBusiness
    {
        Order_Restaurant_Db db = new Order_Restaurant_Db();

        public List<News> getNews(int count)
        {
            var lst_id = db.News.Max(x => x.ID);

            var lst_pro = new List<News>();
            int dem = 0;
            var random = new Random();
            while (true)
            {
                if (dem == count)
                    break;
                long index = random.Next((int)lst_id);
                var news = db.News.Find(index);
                if (news != null && !lst_pro.Exists(x => x.ID == index))
                {
                    lst_pro.Add(news);
                    dem++;
                }

            }

            return lst_pro;
        }
    }
}