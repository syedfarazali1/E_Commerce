using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce_WebApp.Models
{
    public class Cart
    {   [Key]
        public int Id { get; set; }
        public int Pro_Id { get; set; }
        public string Pro_Name { get; set; }
        public Nullable<int> Pro_Price { get; set; }

        public Nullable<int> Ord_Bill { get; set; }
        public int Ord_Quantity { get; set; }

       
    }
}