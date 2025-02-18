using EcommerceBookApp.DataAccess.Data;
using EcommerceBookApp.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBookApp.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }
        public ICompanyRepository CompanyRepository { get; private set; }
        public IShoppingCartRepository ShoppingCartRepository { get; private set; }
        public IApplicationUserRepository ApplicationUserRepository { get; private set; }
        public IOrderHeaderRepository OrderHeaderRepository { get; private set; }
        public IOrderDetailRepository OrderDetailRepository { get; private set; }

        private readonly ApplicationDbContext _applicationDbContext;
        public UnitOfWork(ApplicationDbContext applicationDbContext) 
        {
            _applicationDbContext = applicationDbContext;
            CategoryRepository = new CategoryRepository(_applicationDbContext);
            ProductRepository = new ProductRepository(_applicationDbContext);
            CompanyRepository = new CompanyRepository(_applicationDbContext);
            ShoppingCartRepository = new ShoppingCartRepository(_applicationDbContext);
            ApplicationUserRepository = new ApplicationUserRepository(_applicationDbContext);
            OrderDetailRepository = new OrderDetailRepository(_applicationDbContext);
            OrderHeaderRepository = new OrderHeaderRepository(_applicationDbContext);
        }

        public void Save()
        {
            _applicationDbContext.SaveChanges();
        }
    }
}
