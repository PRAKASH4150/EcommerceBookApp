﻿using EcommerceBookApp.Models;

namespace EcommerceBookApp.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}
