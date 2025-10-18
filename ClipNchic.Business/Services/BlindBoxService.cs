using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;

namespace ClipNchic.Business.Services
{
    public class BlindBoxService
    {
        private readonly BlindBoxRepo _repo;
        public BlindBoxService(BlindBoxRepo repo) => _repo = repo;

        public async Task<ResponseBlindBoxDTO?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<IEnumerable<ResponseBlindBoxDTO>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<int> AddAsync(BlindBoxCreateDto dto) => await _repo.AddAsync(dto);
        public async Task<int> UpdateAsync(BlindBox box) => await _repo.UpdateAsync(box);
        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}