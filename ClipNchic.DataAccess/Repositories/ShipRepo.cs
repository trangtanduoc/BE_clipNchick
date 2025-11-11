using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ClipNchic.DataAccess.Repositories
{
    public class ShipRepo
    {
        private readonly AppDbContext _context;
        public ShipRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Ship?> GetByIdAsync(int id) =>
            await _context.Ships.FirstOrDefaultAsync(s => s.id == id);

        public async Task<IEnumerable<Ship>> GetAllAsync() =>
            await _context.Ships.ToListAsync();

        public async Task<Ship> AddAsync(ShipCreateDto dto)
        {
            var ship = new Ship
            {
                name = dto.name,
                price = dto.price
            };
            _context.Ships.Add(ship);
            await _context.SaveChangesAsync();
            return ship;
        }

        public async Task<int> UpdateAsync(Ship ship)
        {
            var existing = await _context.Ships.FindAsync(ship.id);
            if (existing == null) return 0;
            existing.name = ship.name;
            existing.price = ship.price;
            _context.Ships.Update(existing);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _context.Ships.FindAsync(id);
            if (entity != null)
            {
                _context.Ships.Remove(entity);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}