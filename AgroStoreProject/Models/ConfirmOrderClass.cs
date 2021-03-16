using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroStoreProject.Models
{
    public class ConfirmOrderClass
    {
        public string ProdID { get; set; }
        public string MobileNumber { get; set; }
        public int OrderID { get; set; }
        public Nullable<double> OrderPrice { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public Nullable<int> OrderCount { get; set; }


        public virtual ProductDetail ProductDetail { get; set; }
        public virtual CustomerDetail CustomerDetail { get; set; }

    }
}