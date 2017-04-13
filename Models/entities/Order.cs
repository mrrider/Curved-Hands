using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TTP_Project.Models.constants;

namespace TTP_Project.Models.entities
{
    [Bind(Exclude = "OrderId")]
    public class Order
    {
        [ScaffoldColumn(false)]
        public int OrderId { get; set; }

        [ScaffoldColumn(false)]
        public DateTime OrderDate { get; set; }

        public DateTime CompleteDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string OrderItemsIds { get; set; }

        public string OrderTitle { get; set; }

        public string DetailDescription { get; set; }

        [ScaffoldColumn(false)]
        public decimal Total { get; set; }
        
        public ApplicationUser Customer { get; set; }
        
        public string ToString(Order order)
        {
            StringBuilder bob = new StringBuilder();

            bob.Append("<p>Order Information for Order: " + order.OrderId + "<br>Placed at: " + order.OrderDate + "</p>").AppendLine();
            bob.Append("<h1>" + order.OrderTitle + "</h1>");
            bob.Append("<p>Name: " + order.Customer.FistName + " " + order.Customer.LastName + "<br>");
            bob.Append("<br>").AppendLine();
            bob.Append("<Table>").AppendLine();
             
            string header = "<tr> <th>Item Name</th>" + "<th>Quantity</th>" + "<th>Price</th> <th></th> </tr>";
            bob.Append(header).AppendLine();

            bob.Append("</Table>");
            bob.Append("<b>");

            string footer = string.Format("{0,-12}{1,12}\n",
                                          "Total", order.Total);
            bob.Append(footer).AppendLine();
            bob.Append("</b>");

            return bob.ToString();
        }
    }
  
}