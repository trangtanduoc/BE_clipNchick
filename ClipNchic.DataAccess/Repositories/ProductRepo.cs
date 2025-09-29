﻿using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ClipNchic.DataAccess.Repositories
{
    public class ProductRepo
    {
        private readonly AppDbContext _context;
        public ProductRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync() =>
            await _context.Products
                .Include(p => p.User)
                .Include(p => p.Collection)
                .Include(p => p.Base)
                .Include(p => p.Model)
                .Include(p => p.ProductPics)
                .Include(p => p.CharmProducts)
                    .ThenInclude(cp => cp.Charm)
                .ToListAsync();
        public async Task<Product?> GetProductByIdAsync(int id) =>
             await _context.Products
                .Include(p => p.User)
                .Include(p => p.Collection)
                .Include(p => p.Base)
                .Include(p => p.Model)
                .Include(p => p.ProductPics)
                .Include(p => p.CharmProducts)
                    .ThenInclude(cp => cp.Charm)
                .FirstOrDefaultAsync(p => p.id == id);
        public async Task<int> AddProductAsync(ProductDto dto)
        {
            var product = new Product
            {
                title = dto.Title,
                descript = dto.Descript,
                price = dto.Price,
                stock = dto.Stock,
                status = dto.Status,
            };
            _context.Products.Add(product);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateProductAsync(Product product)
        {
            var existing = await _context.Products.FindAsync(product.id);
            if (existing == null) return 0;
            existing.title = product.title;
            existing.descript = product.descript;
            existing.price = product.price;
            existing.stock = product.stock;
            existing.status = product.status;
            _context.Products.Update(existing);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

    }
}
