using EcommerceBookApp.DataAccess.Data;
using EcommerceBookApp.DataAccess.Repository.IRepository;
using EcommerceBookApp.Models;

namespace EcommerceBookApp.DataAccess.Repository
{
    public class ShoppingCartRepository :Repository<ShoppingCart>, IShoppingCartRepository  
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ShoppingCartRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }


        public void Update(ShoppingCart shoppingCart)
        {
            _applicationDbContext.ShoppingCarts.Update(shoppingCart);
        }
    }
}
