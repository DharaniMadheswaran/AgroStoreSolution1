using AgroStoreProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgroStoreProject.Controllers
{
    public class CustomerController : Controller
    {
        AgroStoreEntitiesNew db = new AgroStoreEntitiesNew();

        // GET: Customer

        public ActionResult CustomerSignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerSignUp([Bind(Include = "CustFirstName,CustLastName,CustMobileNumber,CustEmailId,CustPwd,AddressLine1,AddressLine2,District,Pincode")] CustomerDetail customerDetail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.CustomerDetails.Add(customerDetail);
                    db.SaveChanges();
                    return RedirectToAction("CustomerLogin", "Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error!" + ex.Message;
            }


            return View(customerDetail);
        }
        public ActionResult ViewProductDetails()
        {
            return View(db.ProductDetails.ToList());
        }


        [Route("Cart")]

        public ActionResult ViewCart()
        {
            try
            {
                if (TempData.Peek("CustomerID") != null)
                {
                    string num = TempData.Peek("CustomerID").ToString();
                    List<Cart> cart = db.Carts.Where(c => c.MobileNumber == num).ToList();
                    // List<Cart> cartItems = db.Carts.ToList();
                    return View(cart);

                }
                else
                {
                    return RedirectToAction("CustomerLogin", "Login");
                }

            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Error!" + ex.Message;
            }
            return View();
        }


        public ActionResult AddToCart(string Pid)
        {
            try
            {
                if (TempData.Peek("CustomerID") != null)
                {
                    string mobileNum = TempData.Peek("CustomerID").ToString();
                    ProductDetail product = db.ProductDetails.FirstOrDefault(id => id.ProdID == Pid);
                    CustomerDetail customer = db.CustomerDetails.Where(c => c.CustMobileNumber == mobileNum).SingleOrDefault();

                    Item item = new Item();
                    if (product != null)
                    {
                        item.ProdID = product.ProdID;
                        item.CartProdPrice = product.ProdPrice;
                        item.MobileNumber = mobileNum;
                    }
                    return View(item);
                }
                else
                {
                    return RedirectToAction("CustomerLogin", "Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error!" + ex.Message;
            }
            return View();
        }


        [HttpPost]
        public ActionResult AddToCart(Item items)
        {
            string mobileNum = TempData.Peek("CustomerID").ToString();
            try
            {
                ProductDetail product = db.ProductDetails.FirstOrDefault(p => p.ProdID == items.ProdID);
                Cart cart = new Cart();

                cart.ProdID = items.ProdID;
                cart.MobileNumber = mobileNum;
                cart.CartProdPrice = items.CartProdPrice;
                cart.CartProdQty = items.CartProdQty;
                cart.CartBill = cart.CartProdPrice * cart.CartProdQty;
                db.Carts.Add(cart);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error!" + ex.Message;
            }
            return RedirectToAction("HomePage","Login");
        }
        [Route("Order Details")]

        public ActionResult ViewOrderDetails()
        {
            if(TempData.Peek("CustomerID")!=null)
            {
                string mob = TempData.Peek("CustomerID").ToString();
                if (mob != null)
                {
                    List<OrderDetail> orders = db.OrderDetails.Where(o => o.MobileNumber == mob).ToList();
                    return View(orders);
                }
                
            }
            else
            {
                return RedirectToAction("CustomerLogin", "Login");

            }

            return View();
        }

        public ActionResult OrderNow()
        {
            try
            {
                string num = TempData.Peek("CustomerID").ToString();
                CustomerDetail customer = db.CustomerDetails.Where(c => c.CustMobileNumber == num).FirstOrDefault();
                OrderDetail order = new OrderDetail();
                if (num != null)
                {
                    if (customer != null)
                    {

                        order.MobileNumber = num;
                        //customer.AddressLine1 = customer.AddressLine1;
                        //customer.AddressLine2 = customer.AddressLine2;
                        //customer.District = customer.District;
                        //customer.Pincode = customer.Pincode;
                        return View(order);
                    }
                }
                else
                {
                    return RedirectToAction("CustomerLogin", "Login");
                }

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error!" + ex.Message;
            }
            return View();
        }
        [HttpPost]
        public ActionResult OrderNow(OrderDetail order)
        {
            try
            {
                float totalPrice = Convert.ToInt32(TempData.Peek("TotalPrice").ToString());
                string MobNum = TempData.Peek("CustomerID").ToString();
                if (MobNum != null)
                {

                    var cart = db.Carts.Where(c => c.MobileNumber == MobNum ).FirstOrDefault();
                    order.MobileNumber = MobNum;
                    order.ProdID = cart.ProdID;
                    order.OrderPrice = cart.CartBill;
                    order.OrderDate = DateTime.Now;
                    order.OrderStatus = "Ordered";
                    db.OrderDetails.Add(order);
                    db.SaveChanges();
                    return View("OrderPlacedMessage");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error!" + ex.Message;
            }
            return View();
        }
        public ActionResult OrderPlacedMessage()
        {
            return View();
        }
        public ActionResult Delete_From_Cart(int cid)
        {
            if (cid == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(cid);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }
        [HttpPost, ActionName("Delete_From_Cart")]
        public ActionResult Delete(int cid)
        {
            Cart cart = db.Carts.Find(cid);
            db.Carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("ViewCart");
        }

        public ActionResult Search()
        {
            return View(db.ProductDetails.ToList());
        }

        public JsonResult GetSearchData(string SearchBy, string SearchValue)
        {
            List<ProductDetail> products = new List<ProductDetail>();
            if (SearchBy == "ProdCategory")
            {
                try
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    var Pid = SearchValue;
                    products = db.ProductDetails.Where(b => b. ProdCategory== Pid || SearchValue == null).ToList();

                }
                catch (FormatException)
                {
                    ViewBag.error = "Entered Value is not Category of the Book";
                }
                return Json(products, JsonRequestBehavior.AllowGet);
            }

            return Json(products, JsonRequestBehavior.AllowGet);

           
        }
    }

    //public ActionResult Add_Address(string num)
    //{
    //    var customerData = db.CustomerDetails.Where(x => x.CustMobileNumber == num).FirstOrDefault();
    //    if (customerData != null)
    //    {
    //        TempData["CustomerID"] = num;
    //        TempData.Keep();
    //        return View(customerData);
    //    }
    //    return View();
    //}

    //[HttpPost]
    //public ActionResult Add_Address(CustomerDetail customer)
    //{
    //    string id = TempData["CustomerID"].ToString();
    //    try
    //    {
    //        var data = db.CustomerDetails.Where(x => x.CustMobileNumber == id).FirstOrDefault();
    //        if (TempData.Peek("CustomerID") != null)
    //        {
    //            if (data != null)
    //            {
    //                data.AddressLine1 = customer.AddressLine1;
    //                data.AddressLine2 = customer.AddressLine2;
    //                data.District = customer.District;
    //                data.Pincode = customer.Pincode;
    //                db.Entry(data).State = EntityState.Modified;
    //                db.SaveChanges();
    //            }
    //            return RedirectToAction("OrderNow");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ViewBag.Msg = "Error!" + ex.Message;
    //    }
    //    return View();
    //}

    //public ActionResult ConfirmOrder(string id)
    //{
    //    try
    //    {
    //        var result = db.OrderDetails.FirstOrDefault(s => s.OrderID.Equals(id));             

    //        // string id1 = TempData.Peek("testerId").ToString();
    //        if (result != null)
    //        {
    //            result.OrderStatus = "Confirmed";
    //            db.SaveChanges();
    //            //var projectTable = db.OrderDetails.Where(s => s.OrderID.Equals(id1));
    //            //var projectTable = dbContext.project_modules.Select(s => s);
    //            return RedirectToAction("ViewOrderDetails");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ViewBag.ErrorMessage = "Error Occured" + ex.Message;
    //    }
    //    return View();
    //}

}