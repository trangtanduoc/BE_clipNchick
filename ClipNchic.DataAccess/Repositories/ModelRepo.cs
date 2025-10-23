using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ClipNchic.DataAccess.Repositories
{
    public class ModelRepo
    {
        private readonly AppDbContext _context;
        public ModelRepo(AppDbContext context) => _context = context;

        public async Task<Model?> GetByIdAsync(int id) =>
            await _context.Models.FirstOrDefaultAsync(m => m.id == id);

        public async Task<IEnumerable<Model>> GetAllAsync() =>
            await _context.Models.ToListAsync();

        public async Task<int> AddAsync(ModelCreateDto dto)
        {
            var entity = new Model
            {
                name = dto.name,
                address = dto.address
            };
            _context.Models.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(ModelUpdateDto dto)
        {
            var existing = await _context.Models.FindAsync(dto.id);
            if (existing == null) return 0;
            existing.name = dto.name;
            existing.address = dto.address;
            _context.Models.Update(existing);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _context.Models.FindAsync(id);
            if (entity != null)
            {
                _context.Models.Remove(entity);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}