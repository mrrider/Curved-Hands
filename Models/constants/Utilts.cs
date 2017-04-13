using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using TTP_Project.Models.entities;

namespace TTP_Project.Models.constants
{
    public static class Utilts
    {

        private static WorkItem frontend = new WorkItem
        {
            Name = "Making frontend",
            Description = "Make all user interface",
            Status = TaskStatus.Initial,
            Price = 50
        };

        private static WorkItem backend = new WorkItem
        {
            Name = "Making backend",
            Description = "Make all backend architecture",
            Status = TaskStatus.Initial,
            Price = 50
        };

        private static WorkItem database = new WorkItem
        {
            Name = "Making database",
            Description = "Make all database config",
            Status = TaskStatus.Initial,
            Price = 50
        };

        private static WorkItem payment = new WorkItem
        {
            Name = "Making payment",
            Description = "Make all payment config",
            Status = TaskStatus.Initial,
            Price = 50
        };

        private static WorkItem branding = new WorkItem
        {
            Name = "Making branding",
            Description = "Make all branding",
            Status = TaskStatus.Initial,
            Price = 50
        };


        public static ICollection<WorkItem>  GenericTasks(TemplateSiteTypes type)
        {

            ICollection<WorkItem> tasks = new Collection<WorkItem>();
            switch (type)
            {
                case TemplateSiteTypes.Blog:
                    tasks.Add(frontend);
                    break;
                case TemplateSiteTypes.VisitCard:
                    tasks.Add(frontend);
                    tasks.Add(backend);
                    break;
                case TemplateSiteTypes.Forum:
                    tasks.Add(frontend);
                    tasks.Add(backend);
                    tasks.Add(database);
                    break;
                case TemplateSiteTypes.ECommerce:
                    tasks.Add(frontend);
                    tasks.Add(backend);
                    tasks.Add(database);
                    tasks.Add(payment);
                    break;
                case TemplateSiteTypes.Enterprise:
                    tasks.Add(frontend);
                    tasks.Add(backend);
                    tasks.Add(database);
                    tasks.Add(payment);
                    tasks.Add(branding);
                    break;
                default:
                    return null;
            }
            return tasks;
        }
    }
}