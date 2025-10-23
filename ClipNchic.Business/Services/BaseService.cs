using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;

namespace ClipNchic.Business.Services
{
    public class BaseService
    {
        private readonly BaseRepo _repo;
        private readonly ImageService _imageService;
        private readonly ModelService _modelService;

        public BaseService(BaseRepo repo, ImageService imageService, ModelService modelService)
        {
            _repo = repo;
            _imageService = imageService;
            _modelService = modelService;
        }

        public async Task<ResponseBaseDTO?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<IEnumerable<ResponseBaseDTO>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<int> AddAsync(BaseCreateDto dto, IFormFile? imageFile = null, IFormFile? modelFile = null)
        {
            dto.modelId = null;

            if (modelFile != null && modelFile.Length > 0)
            {
                var model = await _modelService.CreateModelFromFileAsync(modelFile);
                if (model != null)
                    dto.modelId = model.id;
            }

            var baseId = await _repo.AddAsync(dto);

            if (imageFile != null && imageFile.Length > 0)
            {
                await _imageService.UploadBaseImageAsync(baseId, imageFile);
            }

            return baseId;
        }

        public async Task<int> UpdateAsync(BaseUpdateDto dto) => await _repo.UpdateAsync(dto);
        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}