using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ClipNchic.DataAccess.Repositories
{
    public class BlindPicRepo
    {
        private readonly AppDbContext _context;
        public BlindPicRepo(AppDbContext context) => _context = context;

        public async Task<BlindPic?> GetByIdAsync(int id) =>
            await _context.BlindPics.FirstOrDefaultAsync(b => b.id == id);

        public async Task<IEnumerable<BlindPic>> GetAllAsync() =>
            await _context.BlindPics.ToListAsync();

        public async Task<int> AddAsync(BlindPicCreateDto dto)
        {
            var entity = new BlindPic
            {
                blindId = dto.blindId,
                imageId = dto.imageId
            };
            _context.BlindPics.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(BlindPic pic)
        {
            var existing = await _context.BlindPics.FindAsync(pic.id);
            if (existing == null) return 0;
            existing.blindId = pic.blindId;
            existing.imageId = pic.imageId;
            _context.BlindPics.Update(existing);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _context.BlindPics.FindAsync(id);
            if (entity != null)
            {
                _context.BlindPics.Remove(entity);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}