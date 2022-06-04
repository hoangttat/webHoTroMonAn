using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebOrderTbRestaurant
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Rewrite URL giới thiệu
            routes.MapRoute(
                name: "list food category",
                url: "thuc-don/{Link}-{ID}",
                defaults: new { controller = "Food", action = "FoodCategory", id = UrlParameter.Optional },
                namespaces: new string[] { "WebOrderTbRestaurant.Controllers" }
            );


            //Rewrite URL giới thiệu
            routes.MapRoute(
                name: "about restaurant",
                url: "gioi-thieu",
                defaults: new { controller = "Home", action = "About", id = UrlParameter.Optional },
                namespaces: new string[] { "WebOrderTbRestaurant.Controllers" }
            );
            
            //Rewrite URL thực đơn
            routes.MapRoute(
                name: "news food",
                url: "bai-viet",
                defaults: new { controller = "News", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WebOrderTbRestaurant.Controllers" }
            );
            
            //Rewrite URL món ăn
            routes.MapRoute(
                name: "all food",
                url: "mon-an",
                defaults: new { controller = "Food", action = "Menu", id = UrlParameter.Optional },
                namespaces: new string[] { "WebOrderTbRestaurant.Controllers" }
            );

             //Rewrite URL đặt bàn từ liên hệ
            routes.MapRoute(
                name: "book table from contact",
                url: "lien-he",
                defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional },
                namespaces: new string[] { "WebOrderTbRestaurant.Controllers" }
            );

            //Rewrite URL tìm kiếm món ăn
            routes.MapRoute(
                name: "search food",
                url: "tim-kiem",
                defaults: new { controller = "Home", action = "Search", id = UrlParameter.Optional },
                namespaces: new string[] { "WebOrderTbRestaurant.Controllers" }
            );


            //Rewrite URL đặt bàn có chọn menu
            routes.MapRoute(
                name: "Order menu food",
                url: "dat-ban-menu",
                defaults: new { controller = "Order", action = "BookMenu", id = UrlParameter.Optional },
                namespaces: new string[] { "WebOrderTbRestaurant.Controllers" }
            );


            //Rewrite URL thêm món ăn vào menu
            routes.MapRoute(
                name: "Add menu food",
                url: "my-menu",
                defaults: new { controller = "Order", action = "AddMenu", id = UrlParameter.Optional },
                namespaces: new string[] { "WebOrderTbRestaurant.Controllers" }
            );


            //Rewrite URL chi tiết món ăn
            routes.MapRoute(
                name: "Order table",
                url: "dat-ban",
                defaults: new { controller = "Order", action = "PageOrder", id = UrlParameter.Optional },
                namespaces: new string[] { "WebOrderTbRestaurant.Controllers" }
            );

            //Rewrite URL chi tiết món ăn
            routes.MapRoute(
                name: "Food detail",
                url: "mon-an/{metatitle}-{id}",
                defaults: new { controller = "Food", action = "Detail", id = UrlParameter.Optional },
                namespaces: new string[] { "WebOrderTbRestaurant.Controllers" }
            );

            routes.MapRoute(
               name: "News detail",
               url: "bai-viet/{Metatitle}-{ID}",
               defaults: new { controller = "News", action = "Detail", id = UrlParameter.Optional },
               namespaces: new string[] { "WebOrderTbRestaurant.Controllers" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WebOrderTbRestaurant.Controllers" }
            );
        }
    }
}
