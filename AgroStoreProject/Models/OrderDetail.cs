//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AgroStoreProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetail
    {
        public string MobileNumber { get; set; }
        public string ProdID { get; set; }
        public int OrderID { get; set; }
        public Nullable<double> OrderPrice { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public Nullable<int> OrderCount { get; set; }
    
        public virtual CustomerDetail CustomerDetail { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
    }
}