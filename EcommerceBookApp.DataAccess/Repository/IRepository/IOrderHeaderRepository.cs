using EcommerceBookApp.Models;

namespace EcommerceBookApp.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository: IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
    }
}
