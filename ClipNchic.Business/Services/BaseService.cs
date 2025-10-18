using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;

namespace ClipNchic.Business.Services
{
    public class BaseService
    {
        private readonly BaseRepo _repo;
        public BaseService(BaseRepo repo) => _repo = repo;

        public async Task<ResponseBaseDTO?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<IEnumerable<ResponseBaseDTO>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<int> AddAsync(BaseCreateDto dto) => await _repo.AddAsync(dto);
        public async Task<int> UpdateAsync(BaseUpdateDto dto) => await _repo.UpdateAsync(dto);
        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}