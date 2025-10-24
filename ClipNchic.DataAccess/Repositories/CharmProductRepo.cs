using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ClipNchic.DataAccess.Repositories
{
    public class CharmProductRepo
    {
        private readonly AppDbContext _context;
        public CharmProductRepo(AppDbContext context) => _context = context;

        public async Task<CharmProduct?> GetByIdAsync(int id) =>
            await _context.CharmProducts.FirstOrDefaultAsync(c => c.id == id);

        public async Task<IEnumerable<CharmProduct>> GetAllAsync() =>
            await _context.CharmProducts.ToListAsync();

        public async Task<int> AddAsync(CharmProductCreateDto dto)
        {
            var entity = new CharmProduct
            {
                productId = dto.productId,
                charmId = dto.charmId
            };
            _context.CharmProducts.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(CharmProduct charmProduct)
        {
            var existing = await _context.CharmProducts.FindAsync(charmProduct.id);
            if (existing == null) return 0;
            existing.productId = charmProduct.productId;
            existing.charmId = charmProduct.charmId;
            _context.CharmProducts.Update(existing);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _context.CharmProducts.FindAsync(id);
            if (entity != null)
            {
                _context.CharmProducts.Remove(entity);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}