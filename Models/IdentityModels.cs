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

            ApplicationUser accountManager = new ApplicationUser();
            accountManager.UserName = "accountManager";
            accountManager.Email = "accountManager@accountManager.com";
            accountManager.RoleName = RolesConst.ACCOUNT_MANAGER;
            accountManager.FistName = RolesConst.ACCOUNT_MANAGER;
            accountManager.LastName = RolesConst.ACCOUNT_MANAGER;
            IdentityResult financeResult = userManager.Create(accountManager, "Pas@123");
            success = this.AddUserToRole(userManager, accountManager.Id, RolesConst.ACCOUNT_MANAGER);
            if (!success) return success;

         
            ProductItem blogItem = new ProductItem()
            {
                Name = "Blog Template",
                Price = 200,
                shortDescription = "Amazing blog template",
                Description = "Blog for everyone, lots of features",
                ItemPictureUrl = "http://cssmenumaker.com/sites/default/files/blog_list_images/screen_shot_2013-02-11_at_9.07.59_pm.png",
                InternalImage = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Image/im1.png")),
                Categorie = TemplateSiteTypes.Blog
            };

            ProductItem CVItem = new ProductItem()
            {
                Name = "CV Template",
                Price = 400,
                shortDescription = "Amazing CV template",
                Description = "CV for everyone, lots of features",
                ItemPictureUrl = "http://cssmenumaker.com/sites/default/files/blog_list_images/screen_shot_2013-02-11_at_9.07.59_pm.png",
                InternalImage = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Image/im1.png")),
                Categorie = TemplateSiteTypes.VisitCard
            };

            ProductItem forumItem = new ProductItem()
            {
                Name = "Forum Template",
                Price = 600,
                shortDescription = "Amazing forum template",
                Description = "Forum for everyone, lots of features",
                ItemPictureUrl = "http://cssmenumaker.com/sites/default/files/blog_list_images/screen_shot_2013-02-11_at_9.07.59_pm.png",
                InternalImage = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Image/im1.png")),
                Categorie = TemplateSiteTypes.Forum
            };
            ProductItem ecommerceItem = new ProductItem()
            {
                Name = "ECommerce Template",
                Price = 800,
                shortDescription = "Amazing ECommerce template",
                Description = "ECommerce for everyone, lots of features",
                ItemPictureUrl = "http://cssmenumaker.com/sites/default/files/blog_list_images/screen_shot_2013-02-11_at_9.07.59_pm.png",
                InternalImage = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Image/im1.png")),
                Categorie = TemplateSiteTypes.ECommerce
            };
            ProductItem enterpriceItem = new ProductItem()
            {
                Name = "Enterprice Template",
                Price = 1000,
                shortDescription = "Amazing Enterprice template",
                Description = "Enterprice for everyone, lots of features",
                ItemPictureUrl = "http://cssmenumaker.com/sites/default/files/blog_list_images/screen_shot_2013-02-11_at_9.07.59_pm.png",
                InternalImage = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Image/im1.png")),
                Categorie = TemplateSiteTypes.Enterprise
            };
            this.ProductItems.Add(blogItem);
            this.ProductItems.Add(CVItem);
            this.ProductItems.Add(forumItem);
            this.ProductItems.Add(ecommerceItem);
            this.ProductItems.Add(enterpriceItem);

            Finance fin = new Finance()
            {
                 TransactionName = "start_budget",
                 From = "bank",
                 To = "company",
                ItemDescription = "start_invoice",
                 Date = DateTime.Now,
                 Cost = 10000,
                 Balance = 10000
            };

            this.Finances.Add(fin);
            

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

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.Order> Orders { get; set; }

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.ProductItem> ProductItems { get; set; }

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.WorkItem> WorkItems { get; set; }

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<TTP_Project.Models.entities.Finance> Finances { get; set; }
    }   
}