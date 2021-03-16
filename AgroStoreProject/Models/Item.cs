using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroStoreProject.Models
{
    public class Item
    {
   
        public string ProdID { get; set; }
        public Nullable<double> CartProdPrice { get; set; }
        public Nullable<int> CartProdQty { get; set; }
        public Nullable<double> CartBill { get; set; }
        public string MobileNumber { get; set; }

        public virtual CustomerDetail CustomerDetail { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
    }
}