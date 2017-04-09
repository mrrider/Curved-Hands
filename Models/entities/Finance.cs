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
    [Bind(Exclude = "FinanceId")]
    public class Finance
    {
        [ScaffoldColumn(false)]
        public int FinanceId { get; set; }

        public string TransactionName { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public decimal Cost { get; set; }

        public string itemDescription { get; set; }

        public decimal Balance { get; set; }

        public DateTime Date { get; set; }

        //public string ToString(Order order)
        //{
        //    StringBuilder bob = new StringBuilder();

        //    bob.Append("<p>Order Information for Order: " + order.OrderId + "<br>Placed at: " + order.OrderDate + "</p>").AppendLine();
        //    bob.Append("<p>Name: " + order.customer.FistName + " " + order.customer.LastName + "<br>");
        //    bob.Append("<br>").AppendLine();
        //    bob.Append("<Table>").AppendLine();

        //    string header = "<tr> <th>Item Name</th>" + "<th>Quantity</th>" + "<th>Price</th> <th></th> </tr>";
        //    bob.Append(header).AppendLine();

        //    bob.Append("</Table>");
        //    bob.Append("<b>");

        //    string footer = String.Format("{0,-12}{1,12}\n",
        //                                  "Total", order.Total);
        //    bob.Append(footer).AppendLine();
        //    bob.Append("</b>");

        //    return bob.ToString();
        //}
    }
  
}