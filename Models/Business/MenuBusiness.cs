using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Models.Business
{
    public class MenuBusiness
    {
        Order_Restaurant_Db db = new Order_Restaurant_Db();

        public Footer ListFooter()
        {
            return db.Footers.SingleOrDefault(x => x.Status == true);
        }

        public List<Main_Menu> ListMenu()
        {
            return db.Main_Menu.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }
    }
}