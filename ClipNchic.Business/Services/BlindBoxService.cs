using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;


namespace ClipNchic.Business.Services
{
    public class BlindBoxService
    {
        private readonly BlindBoxRepo _repo;
        private readonly ImageService _imageService;

        public BlindBoxService(BlindBoxRepo repo, ImageService imageService)
        {
            _repo = repo;
            _imageService = imageService;
        }

        public async Task<ResponseBlindBoxDTO?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<IEnumerable<ResponseBlindBoxDTO>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<ResponseBlindBoxDTO?> AddAsync(BlindBoxCreateDto dto, IEnumerable<IFormFile>? images = null)
        {
            var box = await _repo.AddAsync(dto);
            if (images != null)
            {
                foreach (var file in images)
                {
                    if (file == null || file.Length == 0) continue;
                    await _imageService.UploadBlindBoxImageAsync(box.id, file);
                }
            }
            return await _repo.GetByIdAsync(box.id);
        }
        public async Task<int> UpdateAsync(BlindBox box) => await _repo.UpdateAsync(box);
        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}