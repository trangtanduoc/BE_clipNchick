using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;

namespace ClipNchic.Business.Services
{
    public class CharmService
    {
        private readonly CharmRepo _repo;
        public CharmService(CharmRepo repo) => _repo = repo;

        public async Task<ResponseCharmDTO?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<IEnumerable<ResponseCharmDTO>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<int> AddAsync(CharmCreateDto dto) => await _repo.AddAsync(dto);
        public async Task<int> UpdateAsync(Charm charm) => await _repo.UpdateAsync(charm);
        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}