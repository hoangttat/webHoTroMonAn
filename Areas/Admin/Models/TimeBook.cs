using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebOrderTbRestaurant.Areas.Admin.Models
{
    public class TimeBook
    {
        public long id { get; set; }
        public string time { get; set; }
        public SelectList TimeList { get; set; }
    }
}