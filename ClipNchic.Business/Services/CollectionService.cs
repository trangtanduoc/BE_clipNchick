using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;

namespace ClipNchic.Business.Services
{
    public class CollectionService
    {
        private readonly CollectionRepo _repo;
        public CollectionService(CollectionRepo repo) => _repo = repo;

        public async Task<Collection?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<IEnumerable<Collection>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<int> AddAsync(CollectionCreateDto dto) => await _repo.AddAsync(dto);
        public async Task<int> UpdateAsync(Collection collection) => await _repo.UpdateAsync(collection);
        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}