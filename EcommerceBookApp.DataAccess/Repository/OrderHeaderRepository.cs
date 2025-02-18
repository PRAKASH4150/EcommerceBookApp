using EcommerceBookApp.DataAccess.Data;
using EcommerceBookApp.DataAccess.Repository.IRepository;
using EcommerceBookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBookApp.DataAccess.Repository
{
    public class OrderHeaderRepository :Repository<OrderHeader>, IOrderHeaderRepository  
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public OrderHeaderRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }


        public void Update(OrderHeader orderHeader)
        {
            _applicationDbContext.OrderHeaders.Update(orderHeader);
        }
    }
}
