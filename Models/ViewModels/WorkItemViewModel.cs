using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TTP_Project.Models.constants;
using TTP_Project.Models.entities;

namespace TTP_Project.Models.ViewModels
{
    public class WorkItemViewModel
    {
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
         [Required]
        public String Description { get; set; }
         [Required]
        public DateTime DueDate { get; set; }
         [Required]
        public TaskStatus Status { get; set; }
         [Required]
        public virtual ApplicationUser AssignedWorker { get; set; }
        public virtual Project AssignedProject { get; set; }

        public WorkItemViewModel() { }

        public WorkItemViewModel(WorkItem workItem)
        {
            this.Id = workItem.Id;
            this.Name = workItem.Name;
            this.Description = workItem.Description;
            this.DueDate = workItem.DueDate;
            this.Status = workItem.Status;
            this.AssignedWorker = workItem.AssignedWorker;
            this.AssignedProject = workItem.assignedProject;
        }
    }
}