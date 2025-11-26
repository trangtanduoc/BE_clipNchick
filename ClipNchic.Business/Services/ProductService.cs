using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;

namespace ClipNchic.Business.Services;

public class ProductService
{
    private readonly ProductRepo _repo;
    private readonly ImageService _imageService;
    private readonly ModelService _modelService;
    private readonly BaseService _baseService;

    public ProductService(ProductRepo repo, ImageService imageService, ModelService modelService, BaseService baseService)
    {
        _repo = repo;
        _imageService = imageService;
        _modelService = modelService;
        _baseService = baseService;
    }

    public Task<ResponseProductDTO?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task<IEnumerable<ResponseProductDTO>> GetAllAsync() => _repo.GetAllAsync();
    public Task<IEnumerable<ResponseProductDTO>> GetByUserIdAsync(int userId) => _repo.GetByUserIdAsync(userId);

    public async Task<ResponseProductDTO?> AddAsync(ProductCreateDto dto, IEnumerable<IFormFile>? files = null, IFormFile? modelFile = null)
    {
        if (dto.baseId.HasValue)
        {
            var baseExists = await _baseService.GetByIdAsync(dto.baseId.Value);
            if (baseExists == null)
                throw new InvalidOperationException($"Base with id {dto.baseId} does not exist.");
        }

        dto.createDate ??= DateTime.UtcNow;

        if (modelFile != null && modelFile.Length > 0)
        {
            var model = await _modelService.CreateModelFromJsonFileAsync(modelFile);
            if (model != null)
                dto.modelId = model.id;
        }

        var product = await _repo.AddAsync(dto);

        if (files != null)
        {
            foreach (var file in files)
            {
                if (file == null || file.Length == 0) continue;
                await _imageService.UploadProductImageAsync(product.id, file);
            }
        }

        return await _repo.GetByIdAsync(product.id);
    }

    public async Task<int> UpdateAsync(ProductUpdateDto dto, IEnumerable<IFormFile>? files = null, IFormFile? modelFile = null)
    {
        if (dto.baseId.HasValue)
        {
            var baseExists = await _baseService.GetByIdAsync(dto.baseId.Value);
            if (baseExists == null)
                throw new InvalidOperationException($"Base with id {dto.baseId} does not exist.");
        }

        ResponseProductDTO? existing = null;

        if (modelFile != null && modelFile.Length > 0)
        {
            // Prefer updating the existing model when present; otherwise create a new one.
            existing = existing ?? await _repo.GetByIdAsync(dto.id);
            var targetModelId = dto.modelId ?? existing?.modelId;

            if (targetModelId.HasValue)
            {
                var model = await _modelService.UpdateModelFromJsonFileAsync(targetModelId.Value, modelFile);
                dto.modelId = model?.id ?? targetModelId.Value;
            }
            else
            {
                var model = await _modelService.CreateModelFromJsonFileAsync(modelFile);
                if (model != null)
                    dto.modelId = model.id;
            }
        }

        var result = await _repo.UpdateAsync(dto);

        if (result > 0 && files != null)
        {
            existing = existing ?? await _repo.GetByIdAsync(dto.id);
            if (existing?.Images != null)
            {
                foreach (var image in existing.Images)
                {
                    await _imageService.DeleteAsync(image.id);
                }
            }

            foreach (var file in files)
            {
                if (file == null || file.Length == 0) continue;
                await _imageService.UploadProductImageAsync(dto.id, file);
            }
        }

        return result;
    }
    public Task<int> DeleteAsync(int id) => _repo.DeleteProductAsync(id);
}
