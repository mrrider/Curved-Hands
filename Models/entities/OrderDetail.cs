using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTP_Project.Models.entities
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual ProductItem Item { get; set; }
        public virtual Order Order { get; set; }

        public string toString()
        {
            return Item.Name+" " + Quantity + " * "+ Item.Price;
        }
    }
}