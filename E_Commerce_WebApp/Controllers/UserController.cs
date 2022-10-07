using E_Commerce_WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce_WebApp.Controllers
{
    public class UserController : Controller
    {
        E_commerce_ProjectEntities1 db = new E_commerce_ProjectEntities1();
        List<Cart> li = new List<Cart>();
  
        public ActionResult Index()
        {
            TempData["tbl_Categories"] = db.tbl_Categories.ToList();
            TempData["tbl_Product"] = db.tbl_Product.Include("tbl_Categories").ToList();

            List<Cart> li2 = Session["cart"] as List<Cart>;


            if (Session["cart"] == null)
            {
                Session["aa"] = 0.ToString();
            }

            else
            {
                Session["aa"] = li2.Count().ToString();

            }

            TempData.Keep();
            return View();
        }

        [HttpGet]
        public ActionResult Product_Index(int? id)
        {
            if (id != null)
            {
                ViewBag.tbl_Products = db.tbl_Product.Include("tbl_Categories").Where(x => x.Pro_Id == id);

                return View(ViewBag.tbl_Products);

            }
            else
            {
                var tbl_Product = db.tbl_Product.Include("tbl_Categories").ToList();
                return View(tbl_Product);
            }
        }

        [HttpGet]
        public ActionResult Category_Index()
        {
            ViewBag.tbl_Categories = db.tbl_Categories.ToList();



            return View(ViewBag.tbl_Categories);
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            ViewBag.b = db.tbl_Product.Where(x => x.Pro_Id == Id).ToList();
            ViewBag.a = db.tbl_Images.Where(x => x.Pro_Id == Id).ToList();

            return View();
        }

        [HttpGet]
        public ActionResult AddToCart()
        {
            if (Session["User"] != null && Session["User_Password"] != null)
            {
                return View();
            }
            return RedirectToAction("../Admin/User_Login");
        }

        [HttpPost]
        public ActionResult AddToCart(int pro_id, int qty)
        {
            if (Session["User"] != null && Session["User_Password"] != null)
            {
                tbl_Product p = db.tbl_Product.Where(x => x.Pro_Id == pro_id).SingleOrDefault();


                Cart c = new Cart();
                c.Pro_Id = pro_id;
                c.Pro_Name = p.Pro_Name;
                c.Pro_Price = p.Pro_Price;
                c.Ord_Quantity = qty;
                c.Ord_Bill = qty * p.Pro_Price;
                if (Session["cart"] == null)
                {
                    li.Add(c);
                    Session["cart"] = li;

                }

                else
                {
                    List<Cart> li2 = Session["cart"] as List<Cart>;
                    int flag = 0;
                    foreach (var item in li2)
                    {
                        if (item.Pro_Id == c.Pro_Id)
                        {
                            item.Ord_Quantity += c.Ord_Quantity;
                            item.Ord_Bill += c.Ord_Bill;
                            flag = 1;
                        }

                        else
                        {
                            li.Add(c);
                            Session["cart"] = li;
                        }
                    }
                    if (flag == 0)
                    {
                        li2.Add(c);
                    }
                    Session["cart"] = li2;
                }


                if (Session["cart"] != null)
                {
                    int? SubTotal = 0;
                    List<Cart> li2 = Session["cart"] as List<Cart>;
                    foreach (var item in li2)
                    {
                        SubTotal += item.Ord_Bill;
                    }
                    var Shipping = SubTotal * 5 / 100;
                    var Total = SubTotal + Shipping;
                    Session["SubTotal"] = SubTotal;
                    Session["Shipping"] = Shipping;
                    Session["Total"] = Total;
                }


                TempData.Keep();


                return View();
            }
            return RedirectToAction("../Admin/User_Login");

        }

        [HttpGet]
        public ActionResult CheckOut()
        {
            if (Session["User"] != null && Session["User_Password"] != null)
            {
             int id =   Convert.ToInt32(@Session["User_Id"]);
                return View();
            }
            return RedirectToAction("../Admin/User_Login");
            
           
        }

        [HttpPost]
        public ActionResult CheckOut(int od_cardNo, string Cus_Address, string date)
        {
            if (Session["User"] != null && Session["User_Password"] != null)
            {


                if (Session["cart"] != null)
                {
                    List<Cart> li2 = Session["cart"] as List<Cart>;
                    
                    foreach (var item in li2)
                    {


                        tbl_Order od = new tbl_Order();
                        od.Ord_Date = date;
                        od.Ord_Quantity = item.Ord_Quantity.ToString();
                        od.Ord_Bill = item.Ord_Bill;
                        od.Ord_CardNumber = od_cardNo;
                        od.Ord_Status = 0.ToString();
                        od.Cus_Address = Cus_Address;
                        od.Pro_Id = Convert.ToInt16(item.Pro_Id);
                        od.U_Id = Convert.ToInt32(Session["User_Id"]);
                        ViewBag.aa = "Order Complete";
                        db.tbl_Order.Add(od);
                        db.SaveChanges();
                        Session["SubTotal"] = null;
                        Session["Shipping"] = null;
                        Session["Total"] = null;
                        Session["cart"] = null;

                        return RedirectToAction("Index");

                    }
                    //}
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("../Admin/User_Login");
        }

        [HttpGet]
        public ActionResult RemoveCart(int? Id)
        {
            if (Session["cart"] == null)
            {
                Session["SubTotal"] = null;
                Session["Shipping"] = null;
                Session["Total"] = null;
            }
            else
            {

                List<Cart> li2 = Session["cart"] as List<Cart>;
                Cart c = li2.Where(x => x.Pro_Id == Id).FirstOrDefault();
                li2.Remove(c);
                int? s = 0;
                foreach (var item in li2)
                {
                    s += item.Ord_Bill;
                }
                Session["SubTotal"] = s;
                var Shipping = s * 5 / 100;
                Session["Shipping"] = Shipping;
                var Total = s + Shipping;
                Session["Total"] = Total;
            }
            return RedirectToAction("AddToCart");
        }


        public ActionResult ProCat(int? id)
        {
            if (id != null)
            {
                TempData["ProCat"] = db.tbl_Product.Include("tbl_Categories").Where(x => x.Cat_Id == id).ToList();
                TempData.Keep();
                return View(TempData["ProCat"]);
                
            }
            return RedirectToAction("Index");
            

        }

        public ActionResult Shop()
        {


            return View();
        }
        [HttpPost]
        public ActionResult Shop(int price, string color, string size)
        {
            List<tbl_Product> p = db.tbl_Product.Include("tbl_Categories").ToList();
            if (price == 1)
            {
                TempData["tbl_Product"] = db.tbl_Product.Include("tbl_Categories").Where(x => x.Pro_Price >= 0 && x.Pro_Price <= 100).ToList();
                return View(TempData["tbl_Product"]);
                TempData.Keep();
            }
            else if (price == 2)
            {
                TempData["tbl_Product"] = db.tbl_Product.Include("tbl_Categories").Where(x => x.Pro_Price >= 100 && x.Pro_Price <= 200).ToList();
                return View(TempData["tbl_Product"]);
                TempData.Keep();
            }
            else if (price == 3)
            {
                TempData["tbl_Product"] = db.tbl_Product.Include("tbl_Categories").Where(x => x.Pro_Price >= 200 && x.Pro_Price <= 300).ToList();
                return View(TempData["tbl_Product"]);
                TempData.Keep();
            }
            else if (price == 4)
            {
                TempData["tbl_Product"] = db.tbl_Product.Include("tbl_Categories").Where(x => x.Pro_Price >= 300 && x.Pro_Price <= 400).ToList();
                return View(TempData["tbl_Product"]);
                TempData.Keep();
            }
            else if (price == 5)
            {
                TempData["tbl_Product"] = db.tbl_Product.Include("tbl_Categories").Where(x => x.Pro_Price >= 400 && x.Pro_Price <= 500).ToList();
                return View(TempData["tbl_Product"]);
                TempData.Keep();
            }
            else if (price == 6)
            {

                TempData["tbl_Product"] = db.tbl_Product.Include("tbl_Categories").Where(x => x.Pro_Price >= 500 && x.Pro_Price <= 2000).ToList();
                
                return View(TempData["tbl_Product"]);
                TempData.Keep();
            }
            else if (price == 0)
            {
                TempData["tbl_Product"] = db.tbl_Product.Include("tbl_Categories").ToList();
                return View(TempData["tbl_Product"]);
                TempData.Keep();
            }

            return View(p);
            

        }

        public ActionResult Contact()
        {

            return View();

        }
        [HttpPost]
        public ActionResult Contact(string name , string email , string subject, string message)
        {
            tbl_Contact contact = new tbl_Contact();
            contact.Name = name;
            contact.User_Email = email;
            contact.Subject = subject;
            contact.Message = message;
            contact.User_ID = Convert.ToInt32(Session["User_Id"]); 
            db.tbl_Contact.Add(contact);
            db.SaveChanges();
            ViewBag.insert = "Sent";
            return View();

        }

    }
}