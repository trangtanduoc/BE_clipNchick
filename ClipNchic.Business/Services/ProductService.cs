﻿using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
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

        public Task<Product?> GetProductAsync(int id) => _repo.GetProductByIdAsync(id);

        public Task<IEnumerable<Product>> GetProductsAsync() => _repo.GetAllProductsAsync();

        public Task<int> CreateProductAsync(ProductDto product)
        {
            product.CreateDate = DateTime.UtcNow;
            return _repo.AddProductAsync(product);
        }

        public Task<int> UpdateProductAsync(Product product) => _repo.UpdateProductAsync(product);

        public Task<int> DeleteProductAsync(int id) => _repo.DeleteProductAsync(id);
    }
}
