using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Models.Business
{
    public class BookMenuBusiness
    {
        Order_Restaurant_Db db = new Order_Restaurant_Db();

        //Lưu thực đơn khách hàng
        public void InsertMenu(Book_Food bf)
        {
                db.Book_Food.Add(bf);
                db.SaveChanges();
        }

        //Cập nhật tổng tiền
        public void UpdateTotalMoney(long OrderTable_ID, decimal TotalMoney)
        {
            var order = db.OrderTables.Find(OrderTable_ID);
            order.TotalMoney = TotalMoney;
            db.SaveChanges();
        }

        public bool CheckID(long id)
        {
            var book_food = from book in db.Book_Food
                            where book.OrderTable_ID == id
                            select book.OrderTable_ID;

            if (book_food.Count() != 0)
                return true;
            return false;
        }

        public IEnumerable<Book_Food> FindID_OrderTB(long orderTB_ID)
        {
            var book = from bok in db.Book_Food
                       where bok.OrderTable_ID == orderTB_ID
                       select bok;
            return book;
        }

        public IEnumerable<Book_Food> ListAll()
        {
            return db.Book_Food.OrderByDescending(x => x.ID).ToList();
        }

        public bool EditCount(long id, int count)
        {
            try
            {
                var bf = db.Book_Food.Find(id);
                bf.Count = count;

                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool deleteFoodinMenu(long id)
        {

            try
            {
                var bf = db.Book_Food.Find(id);
                db.Book_Food.Remove(bf);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}