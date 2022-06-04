using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Models
{
    public class BookFood
    {
        public long Food_ID { get; set; }
        public string Food_Name { get; set; }
        public int Count { get; set; }
        public Nullable<decimal> TotalMoney { get; set; }
        public string Price { get; set; }
        public OrderTable Order_Table { get; set; }

        public string Food_Category_Name { get; set; }

    }
}