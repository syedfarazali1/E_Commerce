using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce_WebApp.Models
{
    public class ProImg
    {
        public virtual ICollection<tbl_Product> Tbl_Products { get; set; }
        public virtual ICollection<tbl_Images> tbl_Images { get; set; }


    }
}