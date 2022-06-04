using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebOrderTbRestaurant.Models.Business;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Areas.Admin.Controllers
{
    public class SlideController : Controller
    {
        Order_Restaurant_Db db = new Order_Restaurant_Db();
        // GET: Admin/Slide
        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            //var model = new BannerServiceClient().getAllBanner().ToPagedList(page, pagesize); ;
            var model = db.Foods.Where(x => x.Type == "Slide").OrderByDescending(x => x.CreatedDate).ToPagedList(page, pagesize);
            return View(model);
        }

        // GET: Admin/Slide/Create
        public ActionResult Create()
        {
            ViewBag.LstCategory = db.Food_Category.ToList();
            return View();
        }

        // POST: Admin/Slide/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Food entity, HttpPostedFileBase Image)
        {
            try
            {
                //Thêm hình ảnh
                var path = Path.Combine(Server.MapPath("~/Assets/Client/img/food"), Image.FileName);
                if (System.IO.File.Exists(path))
                {
                    string extensionName = Path.GetExtension(Image.FileName);
                    string filename = Image.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName;
                    path = Path.Combine(Server.MapPath("~/Assets/Client/img/food"), filename);
                    Image.SaveAs(path);
                    entity.Image = filename;
                }
                else
                {
                    Image.SaveAs(path);
                    entity.Image = Image.FileName;
                }


                entity.Type = "Slide";
                entity.CreatedDate = DateTime.Now;
                entity.MetaTitle = Str_Metatitle(entity.Name);

                db.Foods.Add(entity);
                db.SaveChanges();
                TempData["message"] = "Thêm mới banner thành công!";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "Thêm banner KHÔNG thành công";
                return RedirectToAction("Index");
            }
        }

        // GET: Admin/Slide/Edit/5
        public ActionResult Edit(long id)
        {
            ViewBag.Banner = db.Foods.Find(id);
            ViewBag.LstCategory = db.Food_Category.ToList();
            return View();
        }

        // POST: Admin/Slide/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Food entity, HttpPostedFileBase Image)
        {
            var banner = db.Foods.Find(entity.ID);

            try
            {
                if (Image != null && Image.FileName != entity.Image)
                {
                    //Xóa file cũ
                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/Client/img/food"), entity.Image));
                    //Thêm hình ảnh
                    var path = Path.Combine(Server.MapPath("~/Assets/Client/img/food"), Image.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        string extensionName = Path.GetExtension(Image.FileName);
                        string filename = Image.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName;
                        path = Path.Combine(Server.MapPath("~/Assets/Client/img/food"), filename);
                        Image.SaveAs(path);
                        entity.Image = filename;
                    }
                    else
                    {
                        Image.SaveAs(path);
                        entity.Image = Image.FileName;
                    }
                }

                banner.Name = entity.Name;
                banner.Ingredient = entity.Ingredient;
                banner.Description = entity.Description;
                banner.Price = entity.Price;
                banner.Food_CategoryID = entity.Food_CategoryID;
                banner.MetaTitle = Str_Metatitle(entity.Name);

                db.SaveChanges();
                TempData["message"] = "Cập nhật banner thành công!";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "Cập nhật banner KHÔNG thành công!";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public JsonResult Delete(long id)
        {
            var slide = db.Foods.Find(id);
            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/Client/img/food"), slide.Image));

            var res = new SlideBusiness().DeleteSlide(id);
           
            if (res)
            {
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false });
            }
        }


        
        public string Str_Metatitle(string str)
        {
            string[] VietNamChar = new string[]
            {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ:/"
            };
            //Thay thế và lọc dấu từng char      
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                {
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]).Replace("“", string.Empty).Replace("”", string.Empty);
                    str = str.Replace("\"", string.Empty).Replace("'", string.Empty).Replace("`", string.Empty).Replace(".", string.Empty).Replace(",", string.Empty);
                    str = str.Replace(".", string.Empty).Replace(",", string.Empty).Replace(";", string.Empty).Replace(":", string.Empty);
                    str = str.Replace("?", string.Empty);
                }
            }
            string str1 = str.ToLower();
            string[] name = str1.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string meta = null;
            //Thêm dấu '-'
            foreach (var item in name)
            {
                meta = meta + item + "-";
            }
            return meta.Trim();
        }
    }
}