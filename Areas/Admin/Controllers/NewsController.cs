using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        // GET: Admin/News
        Order_Restaurant_Db db = new Order_Restaurant_Db();
        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            var model = db.News.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pagesize);
            return View(model);
        }

        // GET: Admin/Slide/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Slide/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(News entity, HttpPostedFileBase Image)
        {
            try
            {
                //Thêm hình ảnh
                var path = Path.Combine(Server.MapPath("~/Assets/Client/img/news"), Image.FileName);
                if (System.IO.File.Exists(path))
                {
                    string extensionName = Path.GetExtension(Image.FileName);
                    string filename = Image.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName;
                    path = Path.Combine(Server.MapPath("~/Assets/Client/img/news"), filename);
                    Image.SaveAs(path);
                    entity.Image = filename;
                }
                else
                {
                    Image.SaveAs(path);
                    entity.Image = Image.FileName;
                }


                entity.CreatedDate = DateTime.Now;
                entity.Status = true;
                entity.View = 0;
                entity.Metatile = Str_Metatitle(entity.Title);

                db.News.Add(entity);
                db.SaveChanges();
                TempData["message"] = "Thêm mới bài viết thành công!";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "Thêm bài viết KHÔNG thành công";
                return RedirectToAction("Index");
            }
        }

        // GET: Admin/Slide/Edit/5
        public ActionResult Edit(long id)
        {
            ViewBag.News = db.News.Find(id);
            return View();
        }

        // POST: Admin/Slide/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(News entity, HttpPostedFileBase Image)
        {
            var news = db.News.Find(entity.ID);

            try
            {
                if (Image != null && Image.FileName != entity.Image)
                {
                    //Xóa file cũ
                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/Client/img/news"), entity.Image));
                    //Thêm hình ảnh
                    var path = Path.Combine(Server.MapPath("~/Assets/Client/img/news"), Image.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        string extensionName = Path.GetExtension(Image.FileName);
                        string filename = Image.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName;
                        path = Path.Combine(Server.MapPath("~/Assets/Client/img/news"), filename);
                        Image.SaveAs(path);
                        entity.Image = filename;
                    }
                    else
                    {
                        Image.SaveAs(path);
                        entity.Image = Image.FileName;
                    }
                }

                news.Title = entity.Title;
                news.Description = entity.Description;
                news.Metatile = Str_Metatitle(entity.Title);

                db.SaveChanges();
                TempData["message"] = "Cập nhật bài viết thành công!";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "Cập nhật bài viết KHÔNG thành công!";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public JsonResult Delete(long id)
        {
            var news = db.News.Find(id);
            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/Client/img/news"), news.Image));

            db.News.Remove(news);
            db.SaveChanges();
            return Json(new { status = true });

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