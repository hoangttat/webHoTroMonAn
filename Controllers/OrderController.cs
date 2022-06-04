using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebOrderTbRestaurant.Models;
using WebOrderTbRestaurant.Models.Business;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Controllers
{
    public class OrderController : Controller
    {
        private const string OrderSesstion = "ordersesstion";//session lấy thông tin ngày giờ số lượng khách của khách đặt bàn
        private const string BookFoodSesstion = "BookFood";//session thực đơn   
        // GET: Order
        public ActionResult Index()
        {
            var or = Session[BookFoodSesstion];
            var list = new List<OrderFood>();
            if (or != null)
            {
                list = (List<OrderFood>)or;
            }
            return View(list);
        }


        //Thêm món ăn vào thực đơn
        public ActionResult AddMenu(long foodId, int quantity)
        {
            var food = new FoodBusiness().FindID(foodId);
            //thêm cookei
            var lstfood = new List<Food>();
            lstfood.Insert(0, food);
            new CookiesManage().SetFood_intoCookie(lstfood, true);
            
            var or = Session[BookFoodSesstion];
            if (or != null)
            {
                var list = (List<OrderFood>)or;
                if (list.Exists(x => x.food.ID == foodId))
                {
                    foreach (var item in list)
                    {
                        if (item.food.ID == foodId)
                        {
                            item.quantity += quantity;
                        }
                    }
                }
                else
                {
                    var item = new OrderFood();
                    item.food = food;
                    item.quantity = quantity;
                    list.Add(item);
                }
            }
            else
            {
                var item = new OrderFood();
                var list = new List<OrderFood>();
                item.food = food;
                item.quantity = quantity;
                list.Add(item);
                Session[BookFoodSesstion] = list;
            }
            return RedirectToAction("Index");
        }

        //Xoá một món ăn trong thực đơn
        public JsonResult Delete(long id)
        {
            var sec = (List<OrderFood>)Session[BookFoodSesstion];
            sec.RemoveAll(x => x.food.ID == id);
            Session[BookFoodSesstion] = sec;
            SetAlert("Xóa món ăn thành công", "success");
            return Json(new
            {
                status = true
            });
        }

        //Xóa thực đơn
        public JsonResult DeleteAll()
        {
            Session[BookFoodSesstion] = null;
            return Json(new
            {
                status = true
            });
        }



        //Sửa số lượng món ăn trong thực đơn
        public JsonResult Edit(string EditFood)
        {
            var ed = new JavaScriptSerializer().Deserialize<List<OrderFood>>(EditFood);
            var orSec = (List<OrderFood>)Session[BookFoodSesstion];

            if (ed.Exists(x => x.quantity <= 0))
            {
                //foreach(var item in orSec)
                //{
                //    var err = ed.SingleOrDefault(x => x.food.ID == item.food.ID);
                //    orSec.RemoveAll(x => x.food.ID == err.food.ID);
                //}
                SetAlert("Số lượng món ăn không thể bằng 0 hoặc nhỏ hơn 0", "error");
                return Json(new
                {
                    status = true
                });
            }
            foreach (var item in orSec)
            {
                var foodid = ed.SingleOrDefault(x => x.food.ID == item.food.ID);
                if (foodid != null)
                {
                    item.quantity = foodid.quantity;
                }

            }

            Session[BookFoodSesstion] = orSec;
            return Json(new
            {
                status = true
            });
        }


        //lấy giá trị ngày đặt bàn, giờ đặt bàn, số lượng khách
        public ActionResult PageOrder()
        {
            var ord = Session[OrderSesstion];
            var list = new BookCustomer();
            if (ord != null)
            {
                list = (BookCustomer)ord;
            }
            ViewBag.order = list;
            var or = Session[BookFoodSesstion];
            var FoodOr = new List<OrderFood>();
            if (or != null)
            {
                FoodOr = (List<OrderFood>)or;
            }
            return View(FoodOr);
        }


        //Đặt bàn không chọn thực đơn
        [HttpPost]
        public ActionResult BookTable(OrderTable entity)
        {
            entity.CreatedDate = DateTime.Now;
            try
            {
                var ins = new OrderBusiness();
                ins.Insert(entity);

                if(Session[BookFoodSesstion] != null)
                {
                    var lstFood = Session[BookFoodSesstion] as List<OrderFood>;

                    var bookFood = new Book_Food();
                    var idOrder = new OrderBusiness().FindIDNew();
                    decimal totalMoney = 0;
                    foreach (var item in lstFood)
                    {
                        bookFood.Food_ID = item.food.ID;
                        bookFood.Count = item.quantity;
                        bookFood.Price = Convert.ToDecimal(item.food.Price);
                        bookFood.OrderTable_ID = idOrder;

                        new BookMenuBusiness().InsertMenu(bookFood);
                        totalMoney += Convert.ToDecimal(item.food.Price * item.quantity);

                    }
                    new BookMenuBusiness().UpdateTotalMoney(idOrder, totalMoney);

                }
                Session[BookFoodSesstion] = null;
                return RedirectToAction("Success");
            }
            catch (Exception e)
            {
                return Redirect("dat-ban");
            }
        }

        /*
            Tài khoản: pool-retaurant@business.example.com
            Mật khẩu: poolrestaurant2022
         */
        //Thanh toán paypal
        //public ActionResult PaymentWithPaypal(string Cancel = null)
        //{
        //    //getting the apiContext  
        //    APIContext apiContext = PayPalModel.GetAPIContext();
        //    try
        //    {
        //        //A resource representing a Payer that funds a payment Payment Method as paypal  
        //        //Payer Id will be returned when payment proceeds or click to pay  
        //        string payerId = Request.Params["PayerID"];
        //        if (string.IsNullOrEmpty(payerId))
        //        {
        //            //this section will be executed first because PayerID doesn't exist  
        //            //it is returned by the create function call of the payment class  
        //            // Creating a payment  
        //            // baseURL is the url on which paypal sendsback the data.  
        //            string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Order/PaymentWithPayPal?";
        //            //here we are generating guid for storing the paymentID received in session  
        //            //which will be used in the payment execution  
        //            var guid = Convert.ToString((new Random()).Next(100000));
        //            //CreatePayment function gives us the payment approval url  
        //            //on which payer is redirected for paypal account payment  
        //            var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
        //            //get links returned from paypal in response to Create function call  
        //            var links = createdPayment.links.GetEnumerator();
        //            string paypalRedirectUrl = null;
        //            while (links.MoveNext())
        //            {
        //                Links lnk = links.Current;
        //                if (lnk.rel.ToLower().Trim().Equals("approval_url"))
        //                {
        //                    //saving the payapalredirect URL to which user will be redirected for payment  
        //                    paypalRedirectUrl = lnk.href;
        //                }
        //            }
        //            // saving the paymentID in the key guid  
        //            Session.Add(guid, createdPayment.id);
        //            return Redirect(paypalRedirectUrl);
        //        }
        //        else
        //        {
        //            // This function exectues after receving all parameters for the payment  
        //            var guid = Request.Params["guid"];
        //            var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
        //            //If executed payment failed then we will show payment failure message to user  
        //            if (executedPayment.state.ToLower() != "approved")
        //            {
        //                return View("FailureView");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("FailureView");
        //    }
        //    //on successful payment, show success page to user.
        //    Session[BookFoodSesstion] = null;
        //    return View("Success");
        //}
        //private PayPal.Api.Payment payment;


        //private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        //{
        //    var paymentExecution = new PaymentExecution()
        //    {
        //        payer_id = payerId
        //    };
        //    this.payment = new Payment()
        //    {
        //        id = paymentId
        //    };
        //    return this.payment.Execute(apiContext, paymentExecution);
        //}


        //private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        //{
        //    //create itemlist and add item objects to it  
        //    var itemList = new ItemList()
        //    {
        //        items = new List<Item>()
        //    };
        //    //Adding Item Details like name, currency, price etc  
        //    var Listcart = (List<OrderFood>)Session[BookFoodSesstion];
        //    var user = Session["user"] as User;
        //    decimal tong = 0;
        //    double tax = 0;
        //    foreach (var item in Listcart)
        //    {
        //        var vnd = Convert.ToDecimal((item.quantity * item.food.Price) / 23060);
        //        var money = Math.Round(Convert.ToDecimal((item.quantity * item.food.Price) / 23060), 1);
        //        itemList.items.Add(new Item()
        //        {
        //            name = Str_ProductName(item.food.Name),
        //            currency = "USD",
        //            price = money.ToString(),
        //            quantity = item.quantity.ToString(),
        //            tax = (0.1 * item.quantity).ToString(),
        //            sku = "product" + item.food.ID.ToString()
        //        });

        //        tax += 0.1 * item.quantity;
        //        tong += vnd;
        //    }

        //    //Lưu đơn hàng và chi tiết đơn hàng
        //    var ord = Session[OrderSesstion] as BookCustomer;
        //    //new OrderBusiness().AddOrderTable(tongtien, user, ord, Listcart);
        //    var payer = new Payer()
        //    {
        //        payment_method = "paypal"
        //    };
        //    // Configure Redirect Urls here with RedirectUrls object  
        //    var redirUrls = new RedirectUrls()
        //    {
        //        cancel_url = redirectUrl + "&Cancel=true",
        //        return_url = redirectUrl
        //    };

            
        //    // Adding Tax, shipping and Subtotal details  
        //    var details = new Details()
        //    {
        //        tax = tax.ToString(),
        //        shipping = "1",
        //        subtotal = Math.Round(tong, 1).ToString()
        //    };
        //    //Final amount with details  
        //    var amount = new Amount()
        //    {
        //        currency = "USD",
        //        total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
        //        details = details
        //    };
        //    var transactionList = new List<Transaction>();
        //    // Adding description about the transaction  
        //    transactionList.Add(new Transaction()
        //    {
        //        description = "Transaction description",
        //        invoice_number = Convert.ToString((new Random()).Next(100000)), //Generate an Invoice No  
        //        amount = amount,
        //        item_list = itemList
        //    });
        //    this.payment = new Payment()
        //    {
        //        intent = "sale",
        //        payer = payer,
        //        transactions = transactionList,
        //        redirect_urls = redirUrls
        //    };
        //    // Create a payment using a APIContext  
        //    return this.payment.Create(apiContext);
        //}


        //public ActionResult FailureView()
        //{
        //    return View();
        //}

        public string Str_ProductName(string str)
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
                meta = meta + item + " ";
            }
            return meta;
        }


        public ActionResult Success()
        {
            return View();
        }

        public void SetAlert(string message, string type)
        {
            //Giống ViewBag
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "Warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}