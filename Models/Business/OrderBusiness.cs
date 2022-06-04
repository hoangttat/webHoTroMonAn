using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Models.Business
{
    public class OrderBusiness
    {
        Order_Restaurant_Db db = new Order_Restaurant_Db();

        //Lưu thông tin khách hàng đặt bàn
        public bool Insert(OrderTable or)
        {
            try
            {
                db.OrderTables.Add(or);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        //Lưu đơn đặt khi thanh toán online
        public void AddOrderTable(decimal TongTien, User user, BookCustomer ord, List<OrderFood> lstFood)
        {
            //lưu đơn đặt
            var order = new OrderTable();
            order.Full_Name = user.Full_Name;
            order.Phone = user.Phone;
            order.Email = user.Email;
            order.CreatedDate = DateTime.Now;
            order.DateBook = ord.BookDate;
            order.TimeBook = ord.Time;
            order.Count_people = ord.Quantity;
            order.TotalMoney = TongTien;
            order.User_ID = user.ID;
            order.Status = true;
            db.OrderTables.Add(order);
            db.SaveChanges();

            //Lưu món ăn
            var order_id = db.OrderTables.Max(x => x.ID);
            foreach (var item in lstFood)
            {
                var detail = new Book_Food();
                detail.Food_ID = item.food.ID;
                detail.Count = item.quantity;
                detail.Price = item.food.Price;
                detail.OrderTable_ID = order_id;

                db.Book_Food.Add(detail);
                db.SaveChanges();
            }
        }

        //Lấy ID vừa mới thêm vào csdl
        public long FindIDNew()
        {
            return db.OrderTables.Max(x => x.ID);
        }

        public IEnumerable<OrderTable> ListAll()
        {
            return db.OrderTables.OrderByDescending(x => x.CreatedDate).ToList();
        }

        public OrderTable FindID(long id)
        {
            return db.OrderTables.Find(id);
        }

        public bool EditOrder(OrderTable or)
        {
            try
            {
                var order = db.OrderTables.Find(or.ID);
                order.Full_Name = or.Full_Name;
                order.Phone = or.Phone;
                order.DateBook = or.DateBook;
                order.TimeBook = or.TimeBook;
                order.Count_people = or.Count_people;
                order.Description = or.Description;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteOrder(long id)
        {
            try
            {
                var or = db.OrderTables.Find(id);
                db.OrderTables.Remove(or);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public OrderTable Find_FullName(long order_id)
        {
            return db.OrderTables.SingleOrDefault(x => x.ID == order_id);
        }
    }
}