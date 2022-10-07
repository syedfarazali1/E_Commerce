using E_Commerce_WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce_WebApp.Controllers
{
    public class AdminController : Controller
    { 
        readonly E_commerce_ProjectEntities1 db = new E_commerce_ProjectEntities1();
        #region Registration&UserLogin

        [HttpGet]
        public ActionResult UserList()
        {
            
            var list = db.tbl_User.ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult User_Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult User_Registration(string name,int age, string gender, string cell, string address, string nic,int stuts, string email, string pass, HttpPostedFileBase imgs)
        {
            tbl_User user = new tbl_User();
            var imgpath = Server.MapPath("~/User_images/");
            imgs.SaveAs(Path.Combine(imgpath, imgs.FileName));
            user.U_Img = imgs.FileName;
            user.U_Address = address;
            user.U_Age = age;
            user.U_cell = cell;
            user.U_Cnic = nic;
            user.U_Email = email;
            user.U_Gender = gender;
            user.U_Name = name;
            user.U_Password = pass;
            user.U_Status = stuts.ToString();
            user.U_UserName = name;
            db.tbl_User.Add(user);
            db.SaveChanges();
            ViewBag.ur = "Registration complete";
            return RedirectToAction("~/User/Index");
            
        }
        public ActionResult User_Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult User_Login(string user ,string pass)
        {
            var Login = db.tbl_User.FirstOrDefault(x => x.U_UserName.Equals(user) && x.U_Password.Equals(pass));
            var admin1 = db.tbl_Admin.FirstOrDefault(x => x.Ad_UserName.Equals(user) && x.Ad_Password.Equals(pass));

            if (admin1 != null)
            {
                ViewBag.Massage = "Login_Successfull";
                Session["Admin"] = user;
                Session["Admin_Password"] = pass;
                ViewBag.Massage = "Login Successfull";
                return RedirectToAction("Index", "Admin");
            }
           else if (Login != null)
            {
                ViewBag.Massage = "Login_Successfull";
                ViewBag.Login = "Login";
                Session["User"] = user;
                Session["User_Password"] = user;
                Session["User_Id"] = Login.U_Id;



                return RedirectToAction("Index", "User");
            }

            else
            {
                ViewBag.msg = "Incorrect";
                return View();
            }

        }

        public ActionResult LogOut()
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                Session["Admin"] = null;
                Session["Admin_Password"] = null;
                return RedirectToAction("User_Login");
            }
            else if(Session["User"] != null && Session["User_Password"] != null)
            {
                Session["User"] = null;
                Session["User_Password"] = null;
                Session["cart"] = null;
                Session["SubTotal"] = null;
                Session["Shipping"] = null;
                Session["Total"] = null;
                return RedirectToAction("User_Login");
             
            }

            return View();
        }

        #endregion

        #region Admin/Login
        public ActionResult Admin()
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                List<tbl_Admin> admin = db.tbl_Admin.ToList();


                return View(admin);
            }
            else
            {
                return RedirectToAction("User_Login");
            }
           
        }



        #endregion

        #region Index
        public ActionResult Index()
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                return View();

            }
            else
            {
                return RedirectToAction("User_Login");
            }

            
        }
        #endregion

        #region Category Index
        public ActionResult Category_Index()
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                List<tbl_Categories> cat = db.tbl_Categories.ToList();


                return View(cat);
            }
            else
            {
                return RedirectToAction("User_Login");
            }
          
        }

        #endregion


        #region Add Category
        [HttpGet]
        public ActionResult Category_Add()
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("User_Login");
            }
            
        }
        [HttpPost]
        public ActionResult Category_Add(string name, string Status, HttpPostedFileBase imgs)
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                tbl_Categories cat = new tbl_Categories();
                var imgpath = Server.MapPath("~/Products_images/Cat/");

                imgs.SaveAs(Path.Combine(imgpath, imgs.FileName));
                cat.Cat_Starus = Status;
                cat.C_Name = name;
                cat.Cat_Images = imgs.FileName;
                db.tbl_Categories.Add(cat);
                db.SaveChanges();
                return RedirectToAction("Category_Index");
            }
            else
            {
                return RedirectToAction("User_Login");
            }
        
        }
        #endregion


        #region Edit Category
        [HttpGet]
        public ActionResult Category_Edit(int id)
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                tbl_Categories categories = db.tbl_Categories.Find(id);
                return View(categories);
            }
            else
            {
                return RedirectToAction("User_Login");
            }
           
        }
        [HttpPost]
        public ActionResult Category_Edit(string name, string Status, HttpPostedFileBase imgs,int id)
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                tbl_Categories cat = db.tbl_Categories.Find(id);

                var imgpath = Server.MapPath("~/Products_images/Cat/");

                imgs.SaveAs(Path.Combine(imgpath, imgs.FileName));

                cat.Cat_Images = imgs.FileName;
                cat.C_Name = name;
                cat.Cat_Starus = Status;

                db.Entry(cat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Category_Index");
            }
            else
            {
                return RedirectToAction("User_Login");
            }
           
        }
        #endregion


        #region Delete Category

        [HttpGet]
        public ActionResult Category_Delete(int id)
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                tbl_Categories categories = db.tbl_Categories.Find(id);
                db.tbl_Categories.Remove(categories);
                db.SaveChanges();
                return RedirectToAction("Category_Index");
            }
            else
            {
                return RedirectToAction("User_Login");
            }
          
        }
        #endregion


        #region Delete/Status Category

        public ActionResult Cat_Status(int? id)
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                var tbl_cat = db.tbl_Categories.Find(id);

                if (tbl_cat.Cat_Starus == 0.ToString())
                {
                    tbl_cat.Cat_Starus = 1.ToString();
                }
                else
                {
                    tbl_cat.Cat_Starus = 0.ToString();
                }
                db.SaveChanges();
                return RedirectToAction("Category_Index");
            }
            else
            {
                return RedirectToAction("User_Login");
            }
    
        }
        #endregion

        #region Product Index
        public ActionResult Product_Index(int? id)
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                if (id != null)
                {
                    var tbl_Products = db.tbl_Product.Include("tbl_Categories").Where(x => x.Pro_Id == id);

                    return View(tbl_Products);

                }
                else
                {
                    var tbl_Product = db.tbl_Product.Include("tbl_Categories").ToList();
                    return View(tbl_Product);
                }
            }
            else
            {
                return RedirectToAction("User_Login");
            }
            
        }
        #endregion


        #region Add Product
        [HttpGet]
        public ActionResult AddProduct()
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                ViewBag.Cat_Id = new SelectList(db.tbl_Categories, "Cat_Id", "C_Name");
                return View();
            }
            else
            {
                return RedirectToAction("User_Login");
            }
            
        }
        [HttpPost]
        public ActionResult AddProduct(tbl_Product Product, HttpPostedFileBase imgs)
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                if (ModelState.IsValid)
                {

                    var imgpath = Server.MapPath("~/Products_images/Front/");

                    imgs.SaveAs(Path.Combine(imgpath, imgs.FileName));

                    Product.Pro_Img = imgs.FileName;

                    ViewBag.PID = Product.Pro_Id;


                    db.tbl_Product.Add(Product);

                    db.SaveChanges();

                    return RedirectToAction("AddImages", new { id = Product.Pro_Id });
                }

                ViewBag.Cat_Id = new SelectList(db.tbl_Categories, "Cat_Id", "C_Name", Product.Cat_Id);
                return View(Product);
            }
            else
            {
                return RedirectToAction("User_Login");
            }
        
        }


        #endregion


        #region Edit Product
        public ActionResult Edit(int? id)
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                tbl_Product tbl_Product = db.tbl_Product.Find(id);
                ViewBag.Cat_Id = new SelectList(db.tbl_Categories, "Cat_Id", "C_Name", tbl_Product.Cat_Id);
                return View(tbl_Product);
            }
            else
            {
                return RedirectToAction("User_Login");
            }
         
        }
         
        [HttpPost]

        public ActionResult Edit(tbl_Product tbl_Product , HttpPostedFileBase imgs)
        {
            if (ModelState.IsValid)
            {

                var imgpath = Server.MapPath("~/Products_images/Front/");

                imgs.SaveAs(Path.Combine(imgpath, imgs.FileName));

                tbl_Product.Pro_Img = imgs.FileName;


                db.Entry(tbl_Product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Product_Index");
            }
            ViewBag.Cat_Id = new SelectList(db.tbl_Categories, "Cat_Id", "C_Name", tbl_Product.Cat_Id);
            return View(tbl_Product);
        }
        #endregion


        #region Delete/Status Product
   
        public ActionResult Status(int? id)
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                var tbl_Product = db.tbl_Product.Find(id);

                if (tbl_Product.Pro_status == 0.ToString())
                {
                    tbl_Product.Pro_status = 1.ToString();
                }
                else
                {
                    tbl_Product.Pro_status = 0.ToString();
                }
                db.SaveChanges();
                return RedirectToAction("Product_Index");
            }
            else
            {
                return RedirectToAction("User_Login");
            }
           
        }
        #endregion


        #region ViewImages    
        [HttpGet]
        public ActionResult ViewImages(int? id)
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                if (id != null)
                {
                    var img = db.tbl_Images.Where(x => x.Pro_Id == id);
                    return View(img);
                }
                else
                {
                    List<tbl_Images> imgs = db.tbl_Images.ToList();
                    return View(imgs);
                }
            }
            else
            {
                return RedirectToAction("User_Login");
            }
         



        }

        #endregion


        #region AddProduct_imgs         
       


        [HttpGet]
        public ActionResult AddImages(int? id )
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                ViewBag.PID = id;

                return View();
            }
            else
            {
                return RedirectToAction("User_Login");
            }
           

        }

        [HttpPost]
        public ActionResult AddImages(HttpPostedFileBase img1, HttpPostedFileBase img2 , HttpPostedFileBase img3 , HttpPostedFileBase img4 ,int Pro_Id)
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                tbl_Images img = new tbl_Images();
                var imgpath = Server.MapPath("~/Products_images/");
                img1.SaveAs(Path.Combine(imgpath, img1.FileName));
                img.Img1 = img1.FileName;

                img2.SaveAs(Path.Combine(imgpath, img2.FileName));
                img.Img2 = img2.FileName;

                img3.SaveAs(Path.Combine(imgpath, img3.FileName));
                img.Img3 = img3.FileName;

                img4.SaveAs(Path.Combine(imgpath, img4.FileName));
                img.Img4 = img4.FileName;
                img.Pro_Id = Pro_Id;

                db.tbl_Images.Add(img);
                db.SaveChanges();
                return RedirectToAction("ViewImages");
            }
            else
            {
                return RedirectToAction("User_Login");
            }
        
        }


        #endregion


        #region EditImages      

        [HttpGet]
        public ActionResult EditImages(int? id)
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                tbl_Images tbl_Images = db.tbl_Images.Find(id);
                ViewBag.prolist = db.tbl_Product.ToList();
                return View(tbl_Images);
            }
            else
            {
                return RedirectToAction("User_Login");
            }

            
        }

        [HttpPost]
        public ActionResult EditImages(HttpPostedFileBase img1, HttpPostedFileBase img2, HttpPostedFileBase img3, HttpPostedFileBase img4, int Pro_Id, int Img_Id)
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                tbl_Images img = db.tbl_Images.Find(Img_Id);
                var imgpath = Server.MapPath("~/Products_images/");
                img1.SaveAs(Path.Combine(imgpath, img1.FileName));
                img.Img1 = img1.FileName;

                img2.SaveAs(Path.Combine(imgpath, img2.FileName));
                img.Img2 = img2.FileName;

                img3.SaveAs(Path.Combine(imgpath, img3.FileName));
                img.Img3 = img3.FileName;

                img4.SaveAs(Path.Combine(imgpath, img4.FileName));
                img.Img4 = img4.FileName;


                img.Pro_Id = Pro_Id;
                db.Entry(img).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewImages");
            }
            else
            {
                return RedirectToAction("User_Login");
            }
         
        }
        #endregion

        #region DeleteImages

        
        public ActionResult DeleteImages(int id)
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                tbl_Images img = db.tbl_Images.Find(id);
                db.tbl_Images.Remove(img);
                db.SaveChanges();
                return RedirectToAction("ViewImages");
            }
            else
            {
                return RedirectToAction("User_Login");
            }
        
        }
        #endregion



        #region ODERTABLE
        [HttpGet]
        public ActionResult ordertable()
        {
            if (Session["Admin"] != null && Session["Admin_Password"] != null)
            {
                var list = db.tbl_Order.ToList();
                return View(list);
            }
            else
            {
                return RedirectToAction("User_Login");
            }
            
        }
        #endregion
    }
}