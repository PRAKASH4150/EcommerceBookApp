using EcommerceBookApp.Models;

namespace EcommerceBookApp.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository: IRepository<Category>
    {
        void Update(Category category);
    }
}
