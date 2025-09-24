using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;

namespace ClipNchic.Business.Services
{
    public class CharmProductService
    {
        private readonly CharmProductRepo _repo;
        public CharmProductService(CharmProductRepo repo) => _repo = repo;

        public async Task<CharmProduct?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<IEnumerable<CharmProduct>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<int> AddAsync(CharmProductCreateDto dto) => await _repo.AddAsync(dto);
        public async Task<int> UpdateAsync(CharmProduct charmProduct) => await _repo.UpdateAsync(charmProduct);
        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}