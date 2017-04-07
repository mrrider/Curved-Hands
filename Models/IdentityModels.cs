using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TTP_Project.Models.entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections.Generic;
using System;
using System.Linq;
using TTP_Project.Models.constants;
using TTP_Project.Models.repository;
using TaskStatus = TTP_Project.Models.constants.TaskStatus;

namespace TTP_Project.Models
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ApplicationRole> Roles { get; set; }

        public DbSet<Catagorie> Catagories { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("ModelBuilder is NULL");
            }

            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<ApplicationRole>().HasKey<string>(r => r.Id).ToTable("AspNetRoles");
            modelBuilder.Entity<ApplicationUser>().HasMany<ApplicationUserRole>((ApplicationUser u) => u.UserRoles);
            modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");
        }

        public bool Seed(ApplicationDbContext context)
        {

            bool success = false;

            ApplicationRoleManager _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));

            success = this.CreateRole(_roleManager, RolesConst.ADMIN, "Global Access");
            if (!success == true) return success;

            success = this.CreateRole(_roleManager, RolesConst.CUSTOMER, "Customer");
            if (!success == true) return success;

            success = this.CreateRole(_roleManager, RolesConst.PROGRAMER, "Make work");
            if (!success == true) return success;

            success = this.CreateRole(_roleManager, RolesConst.OPERATOR, "Proceed Orders");
            if (!success == true) return success;

            success = this.CreateRole(_roleManager, RolesConst.RESOURSE_MANAGER, "Manage Resourses");
            if (!success == true) return success;
            
            success = this.CreateRole(_roleManager, RolesConst.MANAGER, "Manager");
            if (!success == true) return success;

            success = this.CreateRole(_roleManager, RolesConst.ORDER_OPERATOR, "Order Manaher");
            if (!success == true) return success;

            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            ApplicationUser user = new ApplicationUser();
            user.UserName = "admin@admin.com";
            user.Email = "admin@admin.com";
            user.RoleName = RolesConst.ADMIN;
            user.FistName = RolesConst.ADMIN;
            user.LastName = RolesConst.ADMIN;
            IdentityResult result = userManager.Create(user, "Pas@123");
            success = this.AddUserToRole(userManager, user.Id, RolesConst.ADMIN);
            if (!success) return success;
         

            ApplicationUser user2 = new Customer();
            user2.UserName = "okpr@gmail.com";
            user2.RoleName = RolesConst.CUSTOMER;
            user2.Email = "okpr@gmail.com";
            user2.FistName = RolesConst.CUSTOMER;
            user2.LastName = RolesConst.CUSTOMER;
            IdentityResult result2 = userManager.Create(user2, "Pas@123");
            success = this.AddUserToRole(userManager, user2.Id, RolesConst.CUSTOMER);
            if (!success) return success;


            ApplicationUser user4 = new ApplicationUser();
            user4.UserName = "programmer@gmail.com";
            user4.Email =   "programmer@gmail.com";
            user4.RoleName = RolesConst.PROGRAMER;
            user4.FistName = RolesConst.PROGRAMER;
            user4.LastName = RolesConst.PROGRAMER;
            user4.EmailConfirmed = true;
            IdentityResult result4 = userManager.Create(user4, "Pas@123");
            success = this.AddUserToRole(userManager, user4.Id, RolesConst.PROGRAMER);
            if (!success) return success;

            ApplicationUser user5 = new ApplicationUser();
            user5.UserName = "operator@gmail.com";
            user5.Email = "operator@gmail.com";
            user5.RoleName = RolesConst.OPERATOR;
            user5.FistName = RolesConst.OPERATOR;
            user5.LastName = RolesConst.OPERATOR;
            IdentityResult result5 = userManager.Create(user5, "Pas@123");
            success = this.AddUserToRole(userManager, user5.Id, RolesConst.OPERATOR);
            if (!success) return success;


            ApplicationUser user15 = new ApplicationUser();
            user15.UserName = "resmen@gmail.com";
            user15.Email = "resmen@gmail.com";
            user15.RoleName = RolesConst.RESOURSE_MANAGER;
            user15.FistName = RolesConst.RESOURSE_MANAGER;
            user15.LastName = RolesConst.RESOURSE_MANAGER;
            IdentityResult result15 = userManager.Create(user15, "Pas@123");
            success = this.AddUserToRole(userManager, user15.Id, RolesConst.RESOURSE_MANAGER);
            if (!success) return success;

            ApplicationUser user10 = new ApplicationUser();
            user10.UserName = "manager@gmail.com";
            user10.Email = "manager@gmail.com";
            user10.RoleName = RolesConst.MANAGER;
            user10.FistName = RolesConst.MANAGER;
            user10.LastName = RolesConst.MANAGER;
            IdentityResult result10 = userManager.Create(user10, "Pas@123");
            success = this.AddUserToRole(userManager, user10.Id, RolesConst.MANAGER);
            if (!success) return success;

            ApplicationUser user12 = new ApplicationUser();
            user12.UserName = "order@gmail.com";
            user12.Email = "order@gmail.com";
            user12.RoleName = RolesConst.ORDER_OPERATOR;
            user12.FistName = RolesConst.ORDER_OPERATOR;
            user12.LastName = RolesConst.ORDER_OPERATOR;
            IdentityResult result12 = userManager.Create(user12, "Pas@123");
            success = this.AddUserToRole(userManager, user12.Id, RolesConst.ORDER_OPERATOR);
            if (!success) return success;

            
            Catagorie card = new Catagorie()
            {
                Name = "VisitCard"
            };

            Catagorie blog = new Catagorie()
            {
                Name = "Blog"
            };

            Catagorie official = new Catagorie()
            {
                Name = "Official"
            };

            Catagorie shop = new Catagorie()
            {
                Name = "Shop"
            };

            Catagorie amazing = new Catagorie()
            {
                Name = "Amazing"
            };

            this.Catagories.Add(card);
            this.Catagories.Add(blog);
            this.Catagories.Add(official);
            this.Catagories.Add(shop);
            this.Catagories.Add(amazing);

            ProductItem item1 = new ProductItem()
            {
                Name = "Template VisitCard",
                Price = 120,
                shortDescription = "VisitCard",
                description = "Some VisitCard for CV",
                Categorie = TemplateSiteTypes.VisitCard
            };

            ProductItem item2 = new ProductItem()
            {
                Name = "Template Blog",
                Price = 200,
                shortDescription = "Funny blog",
                description = "good blog for everyone",
                Categorie = TemplateSiteTypes.Blog
            };

            ProductItem item3 = new ProductItem()
            {
                Name = "Template Official",
                Price = 160,
                shortDescription = "Official",
                description = "Template Official long description",
                Categorie = TemplateSiteTypes.Oficial

            };

            ProductItem item4 = new ProductItem()
            {
                Name = "Amazing Template",
                Price = 224,
                shortDescription = "Amazing",
                description = "Amazing for everyone",
                ItemPictureUrl = "http://cssmenumaker.com/sites/default/files/blog_list_images/screen_shot_2013-02-11_at_9.07.59_pm.png",
                Categorie = TemplateSiteTypes.Amazing
                
            };
            this.ProductItems.Add(item1);
            this.ProductItems.Add(item2);
            this.ProductItems.Add(item3);
            this.ProductItems.Add(item4);
            this.SaveChanges();

            WorkItem workItem = new WorkItem()
            {
                Name = "Frontend",
                Price = 100,
                Description = "Create frontend blog",
                DueDate = DateTime.Today + (new TimeSpan(12, 20, 20)),
                Status = TaskStatus.InProgress,
                AssignedWorker = user4
            };
            this.WorkItems.Add(workItem);

            WorkItem workItem1 = new WorkItem()
            {
                Name = "Шаблон 2",
                Description = "blablabla",
                DueDate = DateTime.Today + (new TimeSpan(12, 20, 20)),
                Status = TaskStatus.InProgress
            };
            this.WorkItems.Add(workItem1);

            WorkItem workItem2 = new WorkItem()
            {
                Name = "Icon",
                Description = "wow",
                DueDate = DateTime.Today + (new TimeSpan(12, 20, 20)),
                Status = TaskStatus.InProgress
            };
            this.WorkItems.Add(workItem2);
          

            Resourse r1 = new Resourse() { 
                 Name = "Meal",
                 Price = 200,
                 Description = "Some meal for staff"
            };

            Resourse r2 = new Resourse()
            {
                Name = "LapTops",
                Price = 1000,
                Description = "Computers for programmers"
            };
            
            this.Resources.Add(r1);
            this.Resources.Add(r2);

            Order or1 = new Order()
            {
                completeDate = new DateTime(2017,5,7) + (new TimeSpan(12, 20, 20)),
                OrderDate = new DateTime(2017, 3, 2) + (new TimeSpan(12, 20, 20)),
                detailDescription = "Order that show bought blog",
                orderStartus = OrderStatus.Initiating,
                Total = 550,
                customer = new Customer()
                {
                    Email = "@",
                    FistName = "A",
                    LastName = "B",
                    RoleName = RolesConst.CUSTOMER,
                    UserName = "Nam"
                },
                orderItems = new List<ProductItem>()

            };

            or1.orderItems.Add(item3);
            or1.orderItems.Add(item4);
            this.Orders.Add(or1);
            
            Order or2 = new Order()
            {
                completeDate = DateTime.Now,
                OrderDate = DateTime.Now,
                detailDescription = "Another description",
                orderStartus = OrderStatus.Processiong,
                Total = 150
            };

            Order or3 = new Order()
            {
                completeDate = DateTime.Now,
                OrderDate = DateTime.Now,
                detailDescription = "Some descriotion",
                orderStartus = OrderStatus.Initiating,
                Total = 600,
            };
        
            this.Orders.Add(or2);
            this.Orders.Add(or3);

            Project project = new Project()
            {
                name = "Creating blog",
                nameProjectManager = "manager@gmail.com",
                costs = 300,
                projectStatus = ProjectStatus.InProgress,
                projectManager = user10,
                order = or1,
                tasks = new List<WorkItem>()
            };

            project.tasks.Add(workItem);
            project.tasks.Add(workItem1);
            this.Projects.Add(project);

            this.SaveChanges();

            return success;

        }

        public bool RoleExists(ApplicationRoleManager roleManager, string name)
        {
            return roleManager.RoleExists(name);
        }

        public bool CreateRole(ApplicationRoleManager _roleManager, string name, string description = "")
        {
            var idResult = _roleManager.Create<ApplicationRole, string>(new ApplicationRole(name, description));
            return idResult.Succeeded;
        }

        public bool AddUserToRole(ApplicationUserManager _userManager, string userId, string roleName)
        {
            var idResult = _userManager.AddToRole(userId, roleName);
       
            return idResult.Succeeded;
        }

        public void ClearUserRoles(ApplicationUserManager userManager, string userId)
        {
            var user = userManager.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();

            currentRoles.AddRange(user.UserRoles);
            foreach (ApplicationUserRole role in currentRoles)
            {
                userManager.RemoveFromRole(userId, role.Role.Name);
            }
        }


        public void RemoveFromRole(ApplicationUserManager userManager, string userId, string roleName)
        {
            userManager.RemoveFromRole(userId, roleName);
        }

        public void DeleteRole(ApplicationDbContext context, ApplicationUserManager userManager, string roleId)
        {
            var roleUsers = context.Users.Where(u => u.UserRoles.Any(r => r.RoleId == roleId));
            var role = context.Roles.Find(roleId);

            foreach (var user in roleUsers)
            {
                this.RemoveFromRole(userManager, user.Id, role.Name);
            }
            context.Roles.Remove(role);
            context.SaveChanges();
        }

        /// <summary>
        /// Context Initializer
        /// </summary>
        public class DropCreateAlwaysInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
        {
            protected override void Seed(ApplicationDbContext context)
            {
                context.Seed(context);

                base.Seed(context);
            }
        }
        

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.Worker> Workers { get; set; }

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.Operator> ApplicationUsers { get; set; }

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.Resourse> Resources { get; set; }

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.Order> Orders { get; set; }

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.ProductItem> ProductItems { get; set; }

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.WorkItem> WorkItems { get; set; }

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.Project> Projects { get; set; }
    }   
}