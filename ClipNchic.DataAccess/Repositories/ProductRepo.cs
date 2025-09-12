using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
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

        public async Task<Product?> GetByIdAsync(int id) =>
            await _context.Products
                .Include(p => p.Model3D) // load model 3D nếu có
                .FirstOrDefaultAsync(p => p.ProductId == id);

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await _context.Products
                .Include(p => p.Model3D)
                .ToListAsync();

        public async Task<int> AddAsync(Product product)
        {
            _context.Products.Add(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity != null)
            {
                _context.Products.Remove(entity);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}
