using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;

namespace ClipNchic.Business.Services
{
    public class BlindPicService
    {
        private readonly BlindPicRepo _repo;
        public BlindPicService(BlindPicRepo repo) => _repo = repo;

        public async Task<BlindPic?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<IEnumerable<BlindPic>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<int> AddAsync(BlindPicCreateDto dto) => await _repo.AddAsync(dto);
        public async Task<int> UpdateAsync(BlindPic pic) => await _repo.UpdateAsync(pic);
        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}