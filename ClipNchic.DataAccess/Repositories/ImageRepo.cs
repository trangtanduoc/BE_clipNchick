using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ClipNchic.DataAccess.Repositories
{
    public class ImageRepo
    {
        private readonly AppDbContext _context;
        public ImageRepo(AppDbContext context) => _context = context;

        public async Task<Image?> GetByIdAsync(int id) =>
            await _context.Images               
                .FirstOrDefaultAsync(i => i.id == id);

        public async Task<IEnumerable<Image>> GetAllAsync(int? baseId, int? charmId, int? blindBoxId, int? productId)
        {
            if (baseId != null)
                return await _context.Images.Where(i => i.baseId == baseId).ToListAsync();
            if (charmId != null)
                return await _context.Images.Where(i => i.charmId == charmId).ToListAsync();
            if (blindBoxId != null)
                return await _context.Images.Where(i => i.blindBoxId == blindBoxId).ToListAsync();
            if (productId != null)
                return await _context.Images.Where(i => i.productId == productId).ToListAsync();
            return await _context.Images.ToListAsync();
        }

        public async Task<Image> AddAsync(ImageCreateDto dto)
        {
            var entity = new Image
            {
                name = dto.name,
                address = dto.address,
                productId = dto.productId,
                baseId = dto.baseId,
                charmId = dto.charmId,
                blindBoxId = dto.blindBoxId
            };
            _context.Images.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> UpdateAsync(Image image)
        {
            var existing = await _context.Images.FindAsync(image.id);
            if (existing == null) return 0;
            existing.name = image.name;
            existing.address = image.address;
            existing.productId = image.productId;
            existing.baseId = image.baseId;
            existing.charmId = image.charmId;
            existing.blindBoxId = image.blindBoxId;
            _context.Images.Update(existing);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _context.Images.FindAsync(id);
            if (entity != null)
            {
                _context.Images.Remove(entity);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}