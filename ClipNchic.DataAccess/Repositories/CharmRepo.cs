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

        public async Task<ResponseCharmDTO?> GetByIdAsync(int id)
        {
            Charm? charm = await _context.Charms.FirstOrDefaultAsync(c => c.id == id);

            var model = await _context.Models.FirstOrDefaultAsync(m => m.id == charm.modelId);
            charm.modelId = model?.id;
            var images = await _context.Images.Where(i => i.charmId == charm.id).ToListAsync();
            var charmProducts = await _context.CharmProducts.Where(cp => cp.charmId == charm.id).ToListAsync();
            var dto = new ResponseCharmDTO
            {
                id = charm.id,
                name = charm.name,
                price = charm.price,
                modelId = charm.modelId,
                Model = model,
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
                CharmProducts = charmProducts.Select(cp => new CharmProduct
                {
                    id = cp.id,
                    charmId = cp.charmId,
                    productId = cp.productId,

                }).ToList()
            };
            return dto;
            }
            

        public async Task<IEnumerable<ResponseCharmDTO>> GetAllAsync() {
           IEnumerable<Charm> charms = await _context.Charms.ToListAsync();
            var charmDtos = new List<ResponseCharmDTO>();
            foreach (var charm in charms)
            {
                var model = await _context.Models.FirstOrDefaultAsync(m => m.id == charm.modelId);
                charm.modelId = model?.id;
                var images = await _context.Images.Where(i => i.charmId == charm.id).ToListAsync();
                var charmProducts = await _context.CharmProducts.Where(cp => cp.charmId == charm.id).ToListAsync();
                var dto = new ResponseCharmDTO
                {
                    id = charm.id,
                    name = charm.name,
                    price = charm.price,
                    modelId = charm.modelId,
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
                    CharmProducts = charmProducts.Select(cp => new CharmProduct
                    {
                        id = cp.id,
                        charmId = cp.charmId,
                        productId = cp.productId,
                        
                    }).ToList()
                    
                };
                

                charmDtos.Add(dto);
            }
        
            return charmDtos;
        }

        public async Task<int> AddAsync(CharmCreateDto dto)
        {
            
            var entity = new Charm
            {
                name = dto.name,
                price = dto.price,
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