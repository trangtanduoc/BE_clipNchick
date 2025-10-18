using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ClipNchic.DataAccess.Repositories
{
    public class ProductRepo
    {
        private readonly AppDbContext _context;
        public ProductRepo(AppDbContext context) => _context = context;

        public async Task<ResponseProductDTO?> GetByIdAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.id == id);
            if (product == null) return null;

            var collection = await _context.Collections.FirstOrDefaultAsync(c => c.id == product.collectId);
            var baseEntity = await _context.Bases.FirstOrDefaultAsync(b => b.id == product.baseId);
            var charmProducts = await _context.CharmProducts.Where(cp => cp.productId == product.id).ToListAsync();      
                var images = await _context.Images.Where(i => i.productId == product.id).ToListAsync();
            

            decimal total = baseEntity?.price ?? 0;
            foreach (var cp in charmProducts)
            {
                if (cp.charmId.HasValue)
                {
                    var charm = await _context.Charms.FirstOrDefaultAsync(c => c.id == cp.charmId);
                    if (charm?.price != null) total += charm.price.Value;
                }
            }
            var finalPrice = product.price ?? total;

            return new ResponseProductDTO
            {
                id = product.id,
                title = product.title,
                descript = product.descript,
                stock = product.stock,
                Totalprice = finalPrice,
                collectId = product.collectId,
                Collection = collection,
                createDate = product.createDate,
                status = product.status,
                baseId = product.baseId,
                Base = baseEntity,
                CharmProducts = charmProducts,
                Images = images
            };
        }

        public async Task<IEnumerable<ResponseProductDTO>> GetAllAsync()
        {
            var products = await _context.Products.ToListAsync();
            var result = new List<ResponseProductDTO>();

            foreach (var product in products)
            {
                var collection = await _context.Collections.FirstOrDefaultAsync(c => c.id == product.collectId);
                var baseEntity = await _context.Bases.FirstOrDefaultAsync(b => b.id == product.baseId);
                var charmProducts = await _context.CharmProducts.Where(cp => cp.productId == product.id).ToListAsync();
 

                    var images = await _context.Images.Where(i => i.productId == product.id).ToListAsync();


                decimal total = baseEntity?.price ?? 0;
                foreach (var cp in charmProducts)
                {
                    if (cp.charmId.HasValue)
                    {
                        var charm = await _context.Charms.FirstOrDefaultAsync(c => c.id == cp.charmId);
                        if (charm?.price != null) total += charm.price.Value;
                    }
                }
                var finalPrice = product.price ?? total;

                result.Add(new ResponseProductDTO
                {
                    id = product.id,
                    title = product.title,
                    descript = product.descript,
                    stock = product.stock,
                    Totalprice = finalPrice,
                    collectId = product.collectId,
                    Collection = collection,
                    baseId = product.baseId,
                    createDate = product.createDate,
                    status = product.status,
                    Base = baseEntity,
                    CharmProducts = charmProducts,
                    Images = images
                });
            }
            return result;
        }

        public async Task<Product> AddAsync(ProductCreateDto dto)
        {
            var entity = new Product
            {
                collectId = dto.collectId,
                title = dto.title,
                descript = dto.descript,
                baseId = dto.baseId,
                price = dto.price,
                userId = dto.userId,
                stock = dto.stock,
                modelId = dto.modelId,
                createDate = dto.createDate,
                status = dto.status
            };
            _context.Products.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> UpdateAsync(ProductUpdateDto dto)
        {
            var existing = await _context.Products.FindAsync(dto.id);
            if (existing == null) return 0;
            existing.collectId = dto.collectId;
            existing.title = dto.title;
            existing.descript = dto.descript;
            existing.baseId = dto.baseId;
            existing.price = dto.price;
            existing.userId = dto.userId;
            existing.stock = dto.stock;
            existing.modelId = dto.modelId;
            existing.createDate = dto.createDate;
            existing.status = dto.status;

            _context.Products.Update(existing);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

    }
}
