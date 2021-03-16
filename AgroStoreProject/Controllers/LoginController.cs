using AgroStoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace AgroStoreProject.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        AgroStoreEntitiesNew db = new AgroStoreEntitiesNew();
        public ActionResult HomePage()
        {
            return View(db.ProductDetails.ToList());
        }
        public ActionResult AdminLogin()
        {

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(AdminLogin adminLogin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    AdminLogin admin = db.AdminLogins.FirstOrDefault(log => log.adMobileNumber.Equals(adminLogin.adMobileNumber) && (log.adPassword.Equals(adminLogin.adPassword)));
                    if (admin != null)
                    {
                        Session["Admin"] = admin.adMobileNumber;
                            TempData["AdminID"] = admin.adMobileNumber;
                        return RedirectToAction("AdminHomePage", "Admin");
                    }
                    ViewBag.Message = false;
                }
            }
            catch(Exception ex)
            {
                ViewBag.Message = true;
            }
                   
                     
            return View();
        }
        public ActionResult CustomerLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerLogin(CustomerDetail customerDetail)
        {
            try
            {
                
                    CustomerDetail customer = db.CustomerDetails.FirstOrDefault(log => log.CustMobileNumber.Equals(customerDetail.CustMobileNumber) && (log.CustPwd.Equals(customerDetail.CustPwd)));
                    if (customer != null)
                    {
                    Session["Customer"] = customer.CustMobileNumber;
                    TempData["CustomerID"] = customer.CustMobileNumber;
                        return RedirectToAction("HomePage", "Login");
                        ViewBag.Message = false;
                    }
                
                
            }
            catch (Exception ex)
            {
                ViewBag.Message = true;
            }

            return View();
        }
        public ActionResult CustomerLogOut()
        {
            Session.Clear();
            return RedirectToAction("HomePage");
        }
        public ActionResult AdminLogOut()
        {
            Session.Clear();
            return RedirectToAction("AdminLogin");

        }
    }
}