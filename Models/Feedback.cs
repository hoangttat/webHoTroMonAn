using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOrderTbRestaurant.Models
{
    public class Feedback
    {
        public string Full_Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string DateBook { get; set; }
        public string TimeBook { get; set; }
        public int Count_people { get; set; }
    }
}