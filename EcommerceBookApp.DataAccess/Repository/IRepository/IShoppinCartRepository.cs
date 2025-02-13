using EcommerceBookApp.Models;

namespace EcommerceBookApp.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository: IRepository<ShoppingCart>
    {
        void Update(ShoppingCart shoppingCart);
    }
}
