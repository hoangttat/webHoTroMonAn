using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Models.Business
{
    public class CookiesManage
    {
        static Order_Restaurant_Db db = new Order_Restaurant_Db();

        public static bool Logined()
        {
            var cookiesClient = HttpContext.Current.Request.Cookies.Get("user");
            if (cookiesClient != null)
            {
                var decodeCookie = CryptorEngine.Decrypt(cookiesClient.Value, true);
                var us = db.Users.FirstOrDefault(x => x.Account == decodeCookie);
                if (us != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static User GetUser()
        {
            var cookiesClient = HttpContext.Current.Request.Cookies.Get("user");
            if (cookiesClient != null)
            {
                var decodeCookie = CryptorEngine.Decrypt(cookiesClient.Value, true);
                
                var us = db.Users.FirstOrDefault(x => x.Account == decodeCookie);
                if (us != null)
                {
                    return us;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public User GetUser_Info()
        {
            var cookiesClient = HttpContext.Current.Request.Cookies.Get("user");
            if (cookiesClient != null)
            {
                var decodeCookie = CryptorEngine.Decrypt(cookiesClient.Value, true);

                var us = db.Users.FirstOrDefault(x => x.Account == decodeCookie);
                if (us != null)
                {
                    return us;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public void SetFood_intoCookie(List<Food> entity, bool first_insert)
        {
            var food = new List<Food>();
            var cookies_food = HttpContext.Current.Request.Cookies.Get("food");
            string objString = "";

            //Thêm cookie
            if (cookies_food != null)
            {
                string FoodString = cookies_food.Value.ToString();
                string[] FoodStringSplit = FoodString.Split('|');
                List<Food> lstfood = new List<Food>();
                foreach (var item in FoodStringSplit)
                {
                    string[] ss = item.Split(',');
                    if (ss[0] != "")
                    {
                        long food_id = Convert.ToInt64(ss[0]);
                        var foodc = db.Foods.Find(food_id);
                        lstfood.Add(foodc);
                    }
                }

                foreach (var item in entity)
                {
                    if (!lstfood.Exists(x => x.ID == item.ID))
                    {
                        lstfood.Insert(0, item);
                    }
                    else if (first_insert == true)
                    {
                        if(lstfood.Exists(x => x.ID == item.ID))
                        {
                            lstfood.RemoveAll(a => a.ID == item.ID);
                        }
                        lstfood.Insert(0, item);
                    }

                }

                foreach (var item in lstfood)
                {
                    objString += string.Join(",", item.ID) + "|";
                }

                HttpContext.Current.Response.Cookies["food"].Value = objString;
                HttpContext.Current.Response.Cookies["food"].Expires = DateTime.Now.AddDays(30);
            }
            else
            {
                foreach (var item in entity)
                {
                    objString += string.Join(",", item.ID) + "|";
                }

                HttpCookie cookie = new HttpCookie("food")
                {
                    Value = objString,
                    Expires = DateTime.Now.AddDays(30)
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
    }
}