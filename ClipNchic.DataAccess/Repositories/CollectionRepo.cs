using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ClipNchic.DataAccess.Repositories
{
    public class CollectionRepo
    {
        private readonly AppDbContext _context;
        public CollectionRepo(AppDbContext context) => _context = context;

        public async Task<Collection?> GetByIdAsync(int id) =>
            await _context.Collections.FirstOrDefaultAsync(c => c.id == id);

        public async Task<IEnumerable<Collection>> GetAllAsync() =>
            await _context.Collections.ToListAsync();

        public async Task<int> AddAsync(CollectionCreateDto dto)
        {
            var entity = new Collection
            {
                name = dto.name,
                descript = dto.descript
            };
            _context.Collections.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Collection collection)
        {
            var existing = await _context.Collections.FindAsync(collection.id);
            if (existing == null) return 0;
            existing.name = collection.name;
            existing.descript = collection.descript;
            _context.Collections.Update(existing);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _context.Collections.FindAsync(id);
            if (entity != null)
            {
                _context.Collections.Remove(entity);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}