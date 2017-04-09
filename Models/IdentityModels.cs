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
using System.IO;
using System.Web;

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

            //Creating ROLES
            ApplicationRoleManager _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));

            success = this.CreateRole(_roleManager, RolesConst.ADMIN, "User managment");
            if (!success == true) return success;

            success = this.CreateRole(_roleManager, RolesConst.CUSTOMER, "Customer");
            if (!success == true) return success;

            success = this.CreateRole(_roleManager, RolesConst.DEVELOPER, "Develop work");
            if (!success == true) return success;

            success = this.CreateRole(_roleManager, RolesConst.ACCOUNT_MANAGER, "Manage finances");
            if (!success == true) return success;

            success = this.CreateRole(_roleManager, RolesConst.ORDER_MANAGER, "Manage Order");
            if (!success == true) return success;

            success = this.CreateRole(_roleManager, RolesConst.PROJECT_MANAGER, "Manage projects");
            if (!success == true) return success;
            

            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            ApplicationUser admin = new ApplicationUser();
            admin.UserName = "admin";
            admin.Email = "admin@admin.com";
            admin.RoleName = RolesConst.ADMIN;
            admin.FistName = RolesConst.ADMIN;
            admin.LastName = RolesConst.ADMIN;
            IdentityResult result = userManager.Create(admin, "Pas@123");
            success = this.AddUserToRole(userManager, admin.Id, RolesConst.ADMIN);
            if (!success) return success;

            ApplicationUser customer = new ApplicationUser();
            customer.UserName = "customer";
            customer.Email = "customer@customer.com";
            customer.RoleName = RolesConst.CUSTOMER;
            customer.FistName = RolesConst.CUSTOMER;
            customer.LastName = RolesConst.CUSTOMER;
            IdentityResult customerResult = userManager.Create(customer, "Pas@123");
            success = this.AddUserToRole(userManager, customer.Id, RolesConst.CUSTOMER);
            if (!success) return success;

            ApplicationUser orderManager = new ApplicationUser();
            orderManager.UserName = "orderManager";
            orderManager.Email = "orderManager@order.com";
            orderManager.RoleName = RolesConst.ORDER_MANAGER;
            orderManager.FistName = RolesConst.ORDER_MANAGER;
            orderManager.LastName = RolesConst.ORDER_MANAGER;
            IdentityResult orderManagerResult = userManager.Create(orderManager, "Pas@123");
            success = this.AddUserToRole(userManager, orderManager.Id, RolesConst.ORDER_MANAGER);
            if (!success) return success;

            ApplicationUser projectManager = new ApplicationUser();
            projectManager.UserName = "projectManager";
            projectManager.Email = "projectManager@project.com";
            projectManager.RoleName = RolesConst.PROJECT_MANAGER;
            projectManager.FistName = RolesConst.PROJECT_MANAGER;
            projectManager.LastName = RolesConst.PROJECT_MANAGER;
            IdentityResult projectManagerResult = userManager.Create(projectManager, "Pas@123");
            success = this.AddUserToRole(userManager, projectManager.Id, RolesConst.PROJECT_MANAGER);
            if (!success) return success;


            ApplicationUser developer = new ApplicationUser();
            developer.UserName = "developer";
            developer.Email = "developer@developer.com";
            developer.RoleName = RolesConst.DEVELOPER;
            developer.FistName = RolesConst.DEVELOPER;
            developer.LastName = RolesConst.DEVELOPER;
            IdentityResult developerResult = userManager.Create(developer, "Pas@123");
            success = this.AddUserToRole(userManager, developer.Id, RolesConst.DEVELOPER);
            if (!success) return success;


            //Catagorie blog = new Catagorie()
            //{
            //    Name = "Blog"
            //};
            //this.Catagories.Add(blog);

            ProductItem blogItem = new ProductItem()
            {
                Name = "Blog Template",
                Price = 200,
                shortDescription = "Amazing blog template",
                description = "Blog for everyone, lots of features",
                ItemPictureUrl = "http://cssmenumaker.com/sites/default/files/blog_list_images/screen_shot_2013-02-11_at_9.07.59_pm.png",
                InternalImage = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Image/im1.png")),
                Categorie = TemplateSiteTypes.Blog
            };
            ProductItem CVItem = new ProductItem()
            {
                Name = "CV Template",
                Price = 400,
                shortDescription = "Amazing CV template",
                description = "CV for everyone, lots of features",
                ItemPictureUrl = "http://cssmenumaker.com/sites/default/files/blog_list_images/screen_shot_2013-02-11_at_9.07.59_pm.png",
                InternalImage = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Image/im1.png")),
                Categorie = TemplateSiteTypes.VisitCard
            };
            this.ProductItems.Add(blogItem);
            this.ProductItems.Add(CVItem);


            //WorkItem workItem = new WorkItem()
            //{
            //    Name = "Frontend",
            //    Price = 100,
            //    Description = "Create frontend blog",
            //    DueDate = DateTime.Today + (new TimeSpan(12, 20, 20)),
            //    Status = TaskStatus.InProgress,
            //    AssignedWorker = user17
            //};
            //this.WorkItems.Add(workItem);

            //WorkItem workItem1 = new WorkItem()
            //{
            //    Name = "Шаблон 2",
            //    Description = "blablabla",
            //    DueDate = DateTime.Today + (new TimeSpan(12, 20, 20)),
            //    Status = TaskStatus.InProgress,
            //    AssignedWorker = user16
            //};
            //this.WorkItems.Add(workItem1);

            //WorkItem workItem2 = new WorkItem()
            //{
            //    Name = "Icon",
            //    Description = "wow",
            //    DueDate = DateTime.Today + (new TimeSpan(12, 20, 20)),
            //    Status = TaskStatus.InProgress
            //};
            //this.WorkItems.Add(workItem2);


            //Resourse r1 = new Resourse()
            //{
            //    Name = "Meal",
            //    Price = 200,
            //    Description = "Some meal for staff"
            //};

            //Resourse r2 = new Resourse()
            //{
            //    Name = "LapTops",
            //    Price = 1000,
            //    Description = "Computers for programmers"
            //};

            //this.Resources.Add(r1);
            //this.Resources.Add(r2);

            //Order or1 = new Order()
            //{
            //    completeDate = new DateTime(2017, 5, 7) + (new TimeSpan(12, 20, 20)),
            //    OrderDate = new DateTime(2017, 3, 2) + (new TimeSpan(12, 20, 20)),
            //    detailDescription = "Order that show bought blog",
            //    orderStartus = OrderStatus.Initiating,
            //    Total = 550,
            //    customer = new Customer()
            //    {
            //        Email = "@",
            //        FistName = "A",
            //        LastName = "B",
            //        RoleName = RolesConst.CUSTOMER,
            //        UserName = "Nam"
            //    },
            //    orderItems = new List<ProductItem>()

            //};

            //or1.orderItems.Add(item3);
            //or1.orderItems.Add(item4);
            //this.Orders.Add(or1);

            //Order or2 = new Order()
            //{
            //    completeDate = DateTime.Now,
            //    OrderDate = DateTime.Now,
            //    detailDescription = "Another description",
            //    orderStartus = OrderStatus.Processiong,
            //    Total = 150
            //};

            //Order or3 = new Order()
            //{
            //    completeDate = DateTime.Now,
            //    OrderDate = DateTime.Now,
            //    detailDescription = "Some descriotion",
            //    orderStartus = OrderStatus.Initiating,
            //    Total = 600,
            //};

            //this.Orders.Add(or2);
            //this.Orders.Add(or3);

            //Project project = new Project()
            //{
            //    name = "Creating blog",
            //    nameProjectManager = "manager@gmail.com",
            //    costs = 300,
            //    projectStatus = ProjectStatus.InProgress,
            //    projectManager = user10,
            //    order = or1,
            //    tasks = new List<WorkItem>()
            //};

            //project.tasks.Add(workItem);
            //project.tasks.Add(workItem1);
            //this.Projects.Add(project);

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
        

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.Operator> ApplicationUsers { get; set; }

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.Order> Orders { get; set; }

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.ProductItem> ProductItems { get; set; }

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.WorkItem> WorkItems { get; set; }

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.Project> Projects { get; set; }
    }   
}