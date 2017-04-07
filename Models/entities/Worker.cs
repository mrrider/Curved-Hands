using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTP_Project.Models.entities
{
    public class Worker : ApplicationUser
    {
        public Double Salary { get; set; }
        public DateTime startWorkDate { get; set; }
        public virtual ICollection<WorkItem> tasks { get; set; }
        

    }
}