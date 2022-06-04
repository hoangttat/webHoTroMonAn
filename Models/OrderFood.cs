using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOrderTbRestaurant.Models.EF;

namespace WebOrderTbRestaurant.Models
{
    public class OrderFood
    {
        public Food food { get; set; }
        public int quantity { get; set; }
    }
}