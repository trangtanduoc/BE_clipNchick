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
            var model = await _modelService.CreateModelFromFileAsync(modelFile);
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

    public Task<int> UpdateAsync(ProductUpdateDto dto) => _repo.UpdateAsync(dto);
    public Task<int> DeleteAsync(int id) => _repo.DeleteProductAsync(id);
}
