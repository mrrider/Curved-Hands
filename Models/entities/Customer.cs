using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTP_Project.Models.entities
{
    public class Customer : ApplicationUser
    {
        public virtual ICollection<Order> orders { get; set; }
    }
}