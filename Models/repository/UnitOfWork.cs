using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TTP_Project.Models.entities;

namespace TTP_Project.Models.repository
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        private GenericRepository<Order> orderRepository;
        private GenericRepository<ApplicationUser> applicationUserRepository;
        private GenericRepository<WorkItem> workItemRepository;
        private GenericRepository<ProductItem> productItemRepository;
        private GenericRepository<Project> projectRepository;
        private GenericRepository<Cart> cartRepository;
        private GenericRepository<Catagorie> catagorieRepository;
        private GenericRepository<Finances> financesRepository;

        public GenericRepository<Catagorie> CatagorieRepositpry
        {
            get
            {

                if (this.catagorieRepository == null)
                {
                    this.catagorieRepository = new GenericRepository<Catagorie>(context);
                }
                return catagorieRepository;
            }
        }

        public GenericRepository<Cart> CartRepository
        {
            get
            {

                if (this.cartRepository == null)
                {
                    this.cartRepository = new GenericRepository<Cart>(context);
                }
                return cartRepository;
            }
        }

        public GenericRepository<ProductItem> ProductItemRepository
        {
            get
            {

                if (this.productItemRepository == null)
                {
                    this.productItemRepository = new GenericRepository<ProductItem>(context);
                }
                return productItemRepository;
            }
        }

        public GenericRepository<Project> ProjectRepository
        {
            get
            {

                if (this.projectRepository == null)
                {
                    this.projectRepository = new GenericRepository<Project>(context);
                }
                return projectRepository;
            }
        }

        public GenericRepository<Order> OrderRepository
        {
            get
            {

                if (this.orderRepository == null)
                {
                    this.orderRepository = new GenericRepository<Order>(context);
                }
                return orderRepository;
            }
        }

        public GenericRepository<WorkItem> WorkItemRepository
        {
            get
            {

                if (this.workItemRepository == null)
                {
                    this.workItemRepository = new GenericRepository<WorkItem>(context);
                }
                return workItemRepository;
            }
        }

        public GenericRepository<ApplicationUser> UserRepository
        {
            get
            {

                if (this.applicationUserRepository == null)
                {
                    this.applicationUserRepository = new GenericRepository<ApplicationUser>(context);
                }
                return applicationUserRepository;
            }
        }

        public GenericRepository<Finances> FinancesRepository
        {
            get
            {

                if (this.financesRepository == null)
                {
                    this.financesRepository = new GenericRepository<Finances>(context);
                }
                return financesRepository;
            }
        }


        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}