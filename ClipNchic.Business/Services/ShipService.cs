using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;

namespace ClipNchic.Business.Services
{
    public class ShipService
    {
        private readonly ShipRepo _repo;
        public ShipService(ShipRepo repo)
        {
            _repo = repo;
        }

        public async Task<Ship?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<IEnumerable<Ship>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<int> AddAsync(ShipCreateDto dto) => await _repo.AddAsync(dto);


        public async Task<int> UpdateAsync(Ship ship) => await _repo.UpdateAsync(ship);

        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}