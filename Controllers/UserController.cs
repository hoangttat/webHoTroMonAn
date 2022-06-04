using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebOrderTbRestaurant.Models.Business;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Controllers
{
    public class UserController : Controller
    {
        Order_Restaurant_Db db = new Order_Restaurant_Db();
        // GET: User
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult frmLogin(User entity)
        {
            var res = db.Users.Count(x => x.Account == entity.Account && x.Password == entity.Password);

            if(res > 0)
            {
                var user = db.Users.FirstOrDefault(x => x.Account == entity.Account && x.Password == entity.Password);
                if(user.Status == false)
                {
                    TempData["message"] = "Tài khoản của bạn đã bị quản trị viên khóa.";
                    return RedirectToAction("login");
                }
                else
                {
                    if (user.Type == 0)
                    {
                        string cookieclient = user.Account;
                        string decodecookieclient = CryptorEngine.Encrypt(cookieclient, true);
                        HttpCookie usercookie = new HttpCookie("user")
                        {
                            Value = decodecookieclient,
                            Expires = DateTime.Now.AddDays(30)
                        };
                        HttpContext.Response.Cookies.Add(usercookie);

                        return Redirect("/");
                    }
                    else
                    {
                        Session["admin"] = user;
                        return Redirect("/Admin/Home");
                    }
                }
            }
            else
            {
                TempData["message"] = "Tài khoản hoặc mật khẩu không đúng.";
                return RedirectToAction("login");
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult frmRegister(User entity)
        {
            if(db.Users.Count(x => x.Account == entity.Account) > 0)
            {
                TempData["message"] = "Tài khoản đăng ký đã tồn tại. Bạn vui lòng đăng ký tài khoản khác.";
                return RedirectToAction("register");
            }
            entity.Password = entity.Password.Trim();
            entity.Type = 0;
            entity.Status = true;
            db.Users.Add(entity);
            db.SaveChanges();
            TempData["message"] = "Đăng ký tài khoản thành công. Bạn vui lòng đăng nhập lại.";
            return RedirectToAction("login");
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            var myCookie = new HttpCookie("user")
            {
                Expires = DateTime.Now.AddDays(-1d)
            };
            Response.Cookies.Add(myCookie);

            var foodCookie = new HttpCookie("food")
            {
                Expires = DateTime.Now.AddDays(-1d)
            };
            Response.Cookies.Add(foodCookie);
            return Redirect("/");
        }

        public ActionResult ChangePass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult frmChangePass(string Old_Pass, string New_Pass)
        {
            var user = WebOrderTbRestaurant.Models.Business.CookiesManage.GetUser();
            if(user.Password.Trim() == Old_Pass)
            {
                var users = db.Users.Find(user.ID);
                users.Password = New_Pass;
                db.SaveChanges();

                Session["user"] = null;
                var myCookie = new HttpCookie("user")
                {
                    Expires = DateTime.Now.AddDays(-1d)
                };
                Response.Cookies.Add(myCookie);
                TempData["message"] = "Đổi mật khẩu thành công. Bạn vui lòng đăng nhập lại.";
                return RedirectToAction("login");
            }
            else
            {
                TempData["message"] = "Mật khẩu cũ không đúng. Vui lòng nhập lại";
                return RedirectToAction("changePass");
            }
            
            
        }

        public ActionResult Food_Favorite(int page = 1, int pagesize = 12)
        {
            var cookiesClient = System.Web.HttpContext.Current.Request.Cookies.Get("user");
            if(cookiesClient == null)
            {
                TempData["message"] = "Bạn vui lòng đăng nhập để tiếp tục.";
                return RedirectToAction("login");
            }
            var user_id = new CookiesManage().GetUser_Info().ID;
            var model = db.Favorites.Where(x => x.User_ID == user_id).OrderByDescending(x => x.ID).ToPagedList(page, pagesize);
            return View(model);
        }
        
        
        public ActionResult Food_View(string type = null, string order = null, int page = 1, int pagesize = 12)
        {
            var cookiesClient = System.Web.HttpContext.Current.Request.Cookies.Get("user");
            if (cookiesClient == null)
            {
                TempData["message"] = "Bạn vui lòng đăng nhập để tiếp tục.";
                return RedirectToAction("login");
            }

            var cookies_food = System.Web.HttpContext.Current.Request.Cookies.Get("food");
            List<Food> lstfood = new List<Food>();
            if (cookies_food != null)
            {
                string FoodString = cookies_food.Value.ToString();
                string[] FoodStringSplit = FoodString.Split('|');
                foreach (var item in FoodStringSplit)
                {
                    string[] ss = item.Split(',');
                    if (ss[0] != "")
                    {
                        long food_id = Convert.ToInt64(ss[0]);
                        var food = db.Foods.Find(food_id);
                        lstfood.Add(food);
                    }

                }

            }

            if (type != null && order != null)
            {
                if (type == "name")
                {
                    if (order == "a-to-z")
                    {
                        lstfood = lstfood.OrderBy(x => x.Name).ToList();
                    }
                    else
                    {
                        lstfood = lstfood.OrderByDescending(x => x.Name).ToList();
                    }
                }
                else
                {
                    if (order == "desc")
                    {
                        lstfood = lstfood.OrderByDescending(x => x.Price).ToList();
                    }
                    else
                    {
                        lstfood = lstfood.OrderBy(x => x.Price).ToList();
                    }
                }

                ViewBag.Type = type;
                ViewBag.Order = order;
                ViewBag.Favorite = db.Favorites.ToList();
                return View(lstfood.ToPagedList(page, pagesize));
            }
            else
            {
                var model = lstfood.OrderByDescending(x => x.ID).ToPagedList(page, pagesize);
                ViewBag.Favorite = db.Favorites.ToList();
                return View(model);
            }

        }
    }
}