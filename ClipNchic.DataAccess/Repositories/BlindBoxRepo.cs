using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.EntityFrameworkCore;


namespace ClipNchic.DataAccess.Repositories
{
    public class BlindBoxRepo
    {
        private readonly AppDbContext _context;
        public BlindBoxRepo(AppDbContext context) => _context = context;

        public async Task<ResponseBlindBoxDTO?> GetByIdAsync(int id)
        {
            var box = await _context.BlindBoxes.FirstOrDefaultAsync(b => b.id == id);
            if (box == null) return null;
            var images = await _context.Images.Where(i => i.blindBoxId == box.id).ToListAsync();
            var collection = await _context.Collections.FirstOrDefaultAsync(c => c.id == box.collectId);
            return new ResponseBlindBoxDTO
            {
                id = box.id,
                collectId = box.collectId,
                Collection = collection,
                name = box.name,
                descript = box.descript,
                price = box.price,
                stock = box.stock,
                status = box.status,
                Images = images
            };
        }

        public async Task<IEnumerable<ResponseBlindBoxDTO>> GetAllAsync()
        {
            var boxes = await _context.BlindBoxes.ToListAsync();
            var result = new List<ResponseBlindBoxDTO>();
            foreach (var box in boxes)
            {
                var collection = await _context.Collections.FirstOrDefaultAsync(c => c.id == box.collectId);
                result.Add(new ResponseBlindBoxDTO
                {
                    id = box.id,
                    collectId = box.collectId,
                    Collection = collection,
                    name = box.name,
                    descript = box.descript,
                    price = box.price,
                    stock = box.stock,
                    status = box.status
                });
            }
            return result;
        }

        public async Task<int> AddAsync(BlindBoxCreateDto dto)
        {
            var entity = new BlindBox
            {
                collectId = dto.collectId,
                name = dto.name,
                descript = dto.descript,
                price = dto.price,
                stock = dto.stock,
                status = dto.status
            };
            _context.BlindBoxes.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(BlindBox box)
        {
            var existing = await _context.BlindBoxes.FindAsync(box.id);
            if (existing == null) return 0;
            existing.collectId = box.collectId;
            existing.name = box.name;
            existing.descript = box.descript;
            existing.price = box.price;
            existing.stock = box.stock;
            existing.status = box.status;
            _context.BlindBoxes.Update(existing);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _context.BlindBoxes.FindAsync(id);
            if (entity != null)
            {
                _context.BlindBoxes.Remove(entity);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}