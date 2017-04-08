using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TTP_Project.Models.constants;

namespace TTP_Project.Models.entities
{
    public class WorkItem
    {
        public int Id  { get; set; }
        public String Name  { get; set; }
        public String Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime DateCreated { get; set; }
        public TaskStatus Status { get; set; }
        public virtual ApplicationUser AssignedWorker { get; set; }
        public virtual Project assignedProject { get; set; }
        public decimal Price { get; set; }

    
    }
}