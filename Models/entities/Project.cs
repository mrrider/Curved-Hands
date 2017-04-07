using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TTP_Project.Models.constants;

namespace TTP_Project.Models.entities
{
    public class Project
    {
        public int id  { get; set; }
        public String name { get; set; }
        public decimal costs { get; set; }
        public ProjectStatus projectStatus { get; set; }
        public virtual ICollection<WorkItem> tasks { get; set; }
        public virtual ApplicationUser projectManager { get; set; }

        public String nameProjectManager { get; set; }
        public virtual Order order {get;set; }
    }
}