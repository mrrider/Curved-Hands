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

        public DateTime completeDate { get; set; }

        public OrderStatus orderStartus { get; set; }

        public string orderItemsIds { get; set; }

        public String detailDescription { get; set; }

        [ScaffoldColumn(false)]
        public decimal Total { get; set; }
        
        public ApplicationUser customer { get; set; }
        
        public string ToString(Order order)
        {
            StringBuilder bob = new StringBuilder();

            bob.Append("<p>Order Information for Order: " + order.OrderId + "<br>Placed at: " + order.OrderDate + "</p>").AppendLine();
            bob.Append("<p>Name: " + order.customer.FistName + " " + order.customer.LastName + "<br>");
            bob.Append("<br>").AppendLine();
            bob.Append("<Table>").AppendLine();
             
            string header = "<tr> <th>Item Name</th>" + "<th>Quantity</th>" + "<th>Price</th> <th></th> </tr>";
            bob.Append(header).AppendLine();

            bob.Append("</Table>");
            bob.Append("<b>");

            string footer = String.Format("{0,-12}{1,12}\n",
                                          "Total", order.Total);
            bob.Append(footer).AppendLine();
            bob.Append("</b>");

            return bob.ToString();
        }
    }
  
}