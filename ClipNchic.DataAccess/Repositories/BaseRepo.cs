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

            var image = await _context.Images.FirstOrDefaultAsync(i => i.baseId == baseEntity.id);
            var model = await _context.Models.FirstOrDefaultAsync(m => m.id == baseEntity.modelId);

            var imageDto = image != null ? new ImageDetailDto
            {
                id = image.id,
                name = image.name,
                address = image.address
            } : null;

            var modelDto = model != null ? new ModelDetailDto
            {
                id = model.id,
                name = model.name,
                address = model.address
            } : null;

            return new ResponseBaseDTO
            {
                id = baseEntity.id,
                name = baseEntity.name,
                color = baseEntity.color,
                price = baseEntity.price,
                Image = imageDto,
                modelId = baseEntity.modelId,
                Model = modelDto
            };
        }

        public async Task<IEnumerable<ResponseBaseDTO>> GetAllAsync()
        {
            IEnumerable<Base> bases = await _context.Bases.ToListAsync();
            var baseDtos = new List<ResponseBaseDTO>();
            foreach (var base1 in bases)
            {
                var model = await _context.Models.FirstOrDefaultAsync(m => m.id == base1.modelId);
                var image = await _context.Images.FirstOrDefaultAsync(i => i.baseId == base1.id);

                var imageDto = image != null ? new ImageDetailDto
                {
                    id = image.id,
                    name = image.name,
                    address = image.address
                } : null;

                var modelDto = model != null ? new ModelDetailDto
                {
                    id = model.id,
                    name = model.name,
                    address = model.address
                } : null;

                var dto = new ResponseBaseDTO
                {
                    id = base1.id,
                    name = base1.name,
                    price = base1.price,
                    color = base1.color,
                    modelId = base1.modelId,
                    Image = imageDto,
                    Model = modelDto
                };
                baseDtos.Add(dto);
            }

            return baseDtos;
        }

        public async Task<Base> AddAsync(BaseCreateDto dto)
        {
            var entity = new Base
            {
                name = dto.name,
                color = dto.color,
                price = dto.price,
                modelId = dto.modelId
            };
            _context.Bases.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
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