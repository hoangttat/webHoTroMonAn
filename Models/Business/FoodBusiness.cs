using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Models.Business
{
    public class FoodBusiness
    {
        Order_Restaurant_Db db = new Order_Restaurant_Db();
        //Chuyển tên món ăn thành metatitle
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
                "ÝỲỴỶỸ:"
            };
            //Thay thế và lọc dấu từng char      
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            string str1 = str.ToLower();
            string[] name = str1.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string meta = null;
            //Thêm dấu '-'
            foreach (var item in name)
            {
                meta = meta + item + "-";
            }
            return meta;
        }

        public bool AddFood(Food entity)
        {
            try
            {
                var fod = new Food();
                fod.Name = entity.Name;
                fod.Image = entity.Image;
                fod.Ingredient = entity.Ingredient;
                fod.CreatedDate = DateTime.Now;
                fod.MetaTitle = Str_Metatitle(entity.Name);
                fod.Description = entity.Description;
                fod.Price = entity.Price;
                fod.Status = true;

                db.Foods.Add(fod);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public Food FindID(long id)
        {
            return db.Foods.Find(id);
        }

        public List<Food> ListAll()
        {
            return db.Foods.Where(x => x.Status == true).OrderBy(x => x.ID).Take(12).ToList();
        }

        public List<Food> ListSlideFood()
        {
            return db.Foods.Where(x => x.Status == true && x.Type=="Slide").OrderByDescending(x => x.CreatedDate <= DateTime.Now).Take(6).ToList();
        }

        public List<Food> PageListFood()
        {
            var cookies_food = HttpContext.Current.Request.Cookies.Get("food");
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
                
                //Thêm những món ăn còn lại
                foreach(var item in db.Foods.ToList())
                {
                    if(!lstfood.Exists(x => x.ID == item.ID))
                    {
                        lstfood.Add(item);
                    }
                }

               
            }
            else
            {
                lstfood = db.Foods.OrderByDescending(x => x.CreatedDate).ToList();
            }
            
            return lstfood;
        }

        public bool EditFood(Food entity)
        {
            try
            {
                var food = db.Foods.Find(entity.ID);
                food.Name = entity.Name;
                food.Ingredient = entity.Ingredient;
                food.Image = entity.Image;
                food.MetaTitle = Str_Metatitle(entity.Name);
                food.Description = entity.Description;
                food.Price = entity.Price;

                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteFood(long id)
        {
            try
            {
                var food = db.Foods.Find(id);
                db.Foods.Remove(food);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Food Find_FoodName(long food_id)
        {
            return db.Foods.SingleOrDefault(x => x.ID == food_id);
        }

        public List<string> searchFood(string nameFood)
        {
            return db.Foods.Where(x => x.Name.Contains(nameFood)).Select(x => x.Name).ToList();
        }

        public List<Food> Search(string namefood)
        {
            var result = (from food in db.Foods
                          where food.Name.Contains(namefood)
                          select food).ToList();
            return result;
        }
    }
}