using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u21447234_HW06.ViewModels
{
    public class OrderVM
    {
        public string ProductName { get; set; }
        public decimal ListPrice { get; set; }
        public int Quantity { get; set; }
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
    }
}