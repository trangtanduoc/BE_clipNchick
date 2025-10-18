using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using Image = ClipNchic.DataAccess.Models.Image;

namespace ClipNchic.DataAccess.Repositories
{
    public class BaseRepo
    {
        private readonly AppDbContext _context;
        public BaseRepo(AppDbContext context) => _context = context;

        public async Task<ResponseBaseDTO?> GetByIdAsync(int id)
        {
            var baseEntity = await _context.Bases.FirstOrDefaultAsync(b => b.id == id);
            if (baseEntity == null) return null;

            var images = await _context.Images.Where(i => i.baseId == baseEntity.id).ToListAsync();
            var model = await _context.Models.FirstOrDefaultAsync(m => m.id == baseEntity.modelId);

            return new ResponseBaseDTO
            {
                id = baseEntity.id,
                name = baseEntity.name,
                color = baseEntity.color,
                price = baseEntity.price,
                Images = images.Select(i => new Image
                {
                    id = i.id,
                    address = i.address,
                    charmId = i.charmId,
                    productId = i.productId,
                    baseId = i.baseId,
                    blindBoxId = i.blindBoxId,
                    name = i.name

                }).ToList(),
                modelId = baseEntity.modelId,
                Model = model
            };
        }

        public async Task<IEnumerable<ResponseBaseDTO>> GetAllAsync()
        {
            IEnumerable<Base> bases = await _context.Bases.ToListAsync();
            var baseDtos = new List<ResponseBaseDTO>();
            foreach (var base1 in bases)
            {
                var model = await _context.Models.FirstOrDefaultAsync(m => m.id == base1.modelId);
                base1.modelId = model?.id;
                var images = await _context.Images.Where(i => i.baseId == base1.id).ToListAsync();
                var dto = new ResponseBaseDTO
                {
                    id = base1.id,
                    name = base1.name,
                    price = base1.price,
                    color = base1.color,
                    modelId = base1.modelId,
                    Images = images.Select(i => new Image
                    {
                        id = i.id,
                        address = i.address,
                        charmId = i.charmId,
                        productId = i.productId,
                        baseId = i.baseId,
                        blindBoxId = i.blindBoxId,
                        name = i.name

                    }).ToList(),

                };
                baseDtos.Add(dto);
            }

            return baseDtos;
        }

        public async Task<int> AddAsync(BaseCreateDto dto)
        {
            var entity = new Base
            {
                name = dto.name,
                color = dto.color,
                price = dto.price,
                modelId = dto.modelId
            };
            _context.Bases.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(BaseUpdateDto dto)
        {
            var existing = await _context.Bases.FindAsync(dto.id);
            if (existing == null) return 0;
            existing.name = dto.name;
            existing.color = dto.color;
            existing.price = dto.price;
            existing.modelId = dto.modelId;
            _context.Bases.Update(existing);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _context.Bases.FindAsync(id);
            if (entity != null)
            {
                _context.Bases.Remove(entity);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}