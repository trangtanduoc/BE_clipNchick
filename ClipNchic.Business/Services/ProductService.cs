﻿using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Repositories;

namespace ClipNchic.Business.Services
{
    public class ProductService
    {
        private readonly ProductRepo _repo;
        public ProductService(ProductRepo repo)
        {
            _repo = repo;
        }

        public Task<Product?> GetProductAsync(int id) => _repo.GetByIdAsync(id);

        public Task<IEnumerable<Product>> GetProductsAsync() => _repo.GetAllAsync();

        public Task<int> CreateProductAsync(Product product)
        {
            product.createDate = DateTime.UtcNow;
            return _repo.AddAsync(product);
        }

        public Task<int> UpdateProductAsync(Product product) => _repo.UpdateAsync(product);

        public Task<int> DeleteProductAsync(int id) => _repo.DeleteAsync(id);
    }
}
