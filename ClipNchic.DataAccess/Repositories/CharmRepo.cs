using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ClipNchic.DataAccess.Repositories
{
    public class CharmRepo
    {
        private readonly AppDbContext _context;
        public CharmRepo(AppDbContext context) => _context = context;

        public async Task<Charm?> GetByIdAsync(int id) =>
            await _context.Charms.FirstOrDefaultAsync(c => c.id == id);

        public async Task<IEnumerable<Charm>> GetAllAsync() =>
            await _context.Charms.ToListAsync();

        public async Task<int> AddAsync(CharmCreateDto dto)
        {
            var entity = new Charm
            {
                name = dto.name,
                price = dto.price,
                imageId = dto.imageId,
                modelId = dto.modelId
            };
            _context.Charms.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Charm charm)
        {
            var existing = await _context.Charms.FindAsync(charm.id);
            if (existing == null) return 0;
            existing.name = charm.name;
            existing.price = charm.price;
            existing.imageId = charm.imageId;
            existing.modelId = charm.modelId;
            _context.Charms.Update(existing);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _context.Charms.FindAsync(id);
            if (entity != null)
            {
                _context.Charms.Remove(entity);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}