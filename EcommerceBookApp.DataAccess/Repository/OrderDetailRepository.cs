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
    public class OrderDetailRepository :Repository<OrderDetail>, IOrderDetailRepository  
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public OrderDetailRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }


        public void Update(OrderDetail orderDetail)
        {
            _applicationDbContext.OrderDetails.Update(orderDetail);
        }
    }
}
