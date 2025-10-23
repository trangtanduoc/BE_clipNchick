using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;

namespace ClipNchic.Business.Services
{
    public class CharmService
    {
        private readonly CharmRepo _repo;
        private readonly ImageService _imageService;
        private readonly ModelService _modelService;

        public CharmService(CharmRepo repo, ImageService imageService, ModelService modelService)
        {
            _repo = repo;
            _imageService = imageService;
            _modelService = modelService;
        }

        public async Task<ResponseCharmDTO?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<IEnumerable<ResponseCharmDTO>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<int> AddAsync(CharmCreateDto dto, IFormFile? imageFile = null, IFormFile? modelFile = null)
        {
            dto.modelId = null;

            if (modelFile != null && modelFile.Length > 0)
            {
                var model = await _modelService.CreateModelFromFileAsync(modelFile);
                if (model != null)
                    dto.modelId = model.id;
            }

            var charmId = await _repo.AddAsync(dto);

            if (imageFile != null && imageFile.Length > 0)
            {
                await _imageService.UploadCharmImageAsync(charmId, imageFile);
            }

            return charmId;
        }

        public async Task<int> UpdateAsync(Charm charm) => await _repo.UpdateAsync(charm);
        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}