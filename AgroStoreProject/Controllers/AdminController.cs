using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AgroStoreProject.Models;
namespace AgroStoreProject.Controllers
{
    public class AdminController : Controller
    {
        AgroStoreEntitiesNew db = new AgroStoreEntitiesNew();
        // GET: Admin
        public ActionResult AdminHomePage()
        {
            if(TempData.Peek("AdminID")!=null)
            {
                return View();

            }
            else
            {
                return RedirectToAction("AdminLogin", "Login");
            }

        }
        [Route("Product Details")]
        public ActionResult AddProductDetails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProductDetails(ProductDetail productDetail)
        {
            try
            {
                string filename = Path.GetFileNameWithoutExtension(productDetail.ProductImage.FileName);
                string extension = Path.GetExtension(productDetail.ProductImage.FileName);
                filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                productDetail.ProdImage = "~/ProductImages/" + filename;
                filename = Path.Combine(Server.MapPath("~/ProductImages/"), filename);
                productDetail.ProductImage.SaveAs(filename);
                using (AgroStoreEntitiesNew agroimg = new AgroStoreEntitiesNew())
                {
                    
                    agroimg.ProductDetails.Add(productDetail);
                    agroimg.SaveChanges();
                    //productDetail.ProdSavings = productDetail.ProdMRP - productDetail.ProdPrice;
                    //agroimg.ProductDetails.Add(productDetail);
                    //agroimg.SaveChanges();

                }
                
            }
            catch(Exception ex)
            {
                ViewBag.Error_Message = "Error Occurred" + ex.Message;
            }
            
            ModelState.Clear();
            return View();
        }

       
        
        public ActionResult ViewAllOrderDetails()
        {
            return View(db.OrderDetails.ToList());
        }


        

        
    }
}