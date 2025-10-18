using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;

namespace ClipNchic.Business.Services
{
    public class ModelService
    {
        private readonly ModelRepo _repo;
        public ModelService(ModelRepo repo) => _repo = repo;

        public async Task<Model?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<IEnumerable<Model>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<int> AddAsync(ModelCreateDto dto) => await _repo.AddAsync(dto);
        public async Task<int> UpdateAsync(ModelUpdateDto dto) => await _repo.UpdateAsync(dto);
        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}