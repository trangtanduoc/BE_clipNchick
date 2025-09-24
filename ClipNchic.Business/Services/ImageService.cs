using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;

namespace ClipNchic.Business.Services
{
    public class ImageService
    {
        private readonly ImageRepo _repo;
        public ImageService(ImageRepo repo) => _repo = repo;

        public async Task<Image?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<IEnumerable<Image>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<int> AddAsync(ImageCreateDto dto) => await _repo.AddAsync(dto);
        public async Task<int> UpdateAsync(Image image) => await _repo.UpdateAsync(image);
        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}