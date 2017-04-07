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

        private static WorkItem first = new WorkItem
        {
            Name = "Test",
            Description = "Make some work",
            Status = TaskStatus.Initial,
            Price = 300

        };

        private static WorkItem makeInterface = new WorkItem
        {
            Name = "Make Interfase",
            Description = "Make",
            Status = TaskStatus.Initial,
            Price = 100
        };


        private static WorkItem makeNewGraphics = new WorkItem
        {
            Name = "New Graphics",
            Description = "Make new graphics",
            Status = TaskStatus.Initial,
            Price = 500
        };

        private static WorkItem makeBackEnd = new WorkItem
        {
            Name = "BackEnd",
            Description = "Make back end ... ",
            Status = TaskStatus.Initial,
            Price = 1500
        };



        private static WorkItem createDataBase = new WorkItem
        {
            Name = "DataBase",
            Description = "It consist something special",
            Status = TaskStatus.Initial,
            Price = 400
        };

        private static WorkItem findResourses = new WorkItem
        {
            Name = "Resourse",
            Description = "Find some something",
            Status = TaskStatus.Initial,
            Price = 700
        };


        private static WorkItem kakaatoHRENY = new WorkItem
        {
            Name = "TSILKOM NORMALNO",
            Description = "Є моменти зроблені є не зроблені",
            Status = TaskStatus.Initial,
            Price = 1200
        };


        private static WorkItem propoyWORK = new WorkItem
        {
            Name = "Some amazing",
            Description = "The one to rule them all",
            Status = TaskStatus.Initial,
            Price = 1000
        };


        private static WorkItem error = new WorkItem
        {
            Name = "Somethin wrong",
            Description ="YOU WROTE SHITCODE",
            Status = TaskStatus.Completed,
            Price = 1
        };

        public static ICollection<WorkItem>  GenericTasks(TemplateSiteTypes type)
        {

            ICollection<WorkItem> tasks = new Collection<WorkItem>();
            switch (type)
            {
                case TemplateSiteTypes.Blog:
                    tasks.Add(makeInterface);
                    tasks.Add(makeBackEnd);
                    break;
                case TemplateSiteTypes.VisitCard:
                    tasks.Add(makeInterface);
                    tasks.Add(makeNewGraphics);
                    break;
                case TemplateSiteTypes.Oficial:
                    tasks.Add(makeInterface);
                    tasks.Add(makeBackEnd);
                    tasks.Add(findResourses);
                    break;
                case TemplateSiteTypes.Shop:
                    tasks.Add(makeBackEnd);
                    tasks.Add(makeNewGraphics);
                    tasks.Add(createDataBase);
                    break;
                case TemplateSiteTypes.Amazing:
                    tasks.Add(createDataBase);
                    tasks.Add(kakaatoHRENY);
                    tasks.Add(makeNewGraphics);
                    tasks.Add(makeInterface);
                    tasks.Add(makeBackEnd);
                    tasks.Add(propoyWORK);
                    break;
                default:
                    tasks.Add(error);
                    break;
            }
            return tasks;
        }
    }
}