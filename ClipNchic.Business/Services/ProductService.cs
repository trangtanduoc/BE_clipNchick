using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;

namespace ClipNchic.Business.Services;

public class ProductService
{
    private readonly ProductRepo _repo;
    private readonly ImageService _imageService;
    private readonly ModelService _modelService;

    public ProductService(ProductRepo repo, ImageService imageService, ModelService modelService)
    {
        _repo = repo;
        _imageService = imageService;
        _modelService = modelService;
    }

    public Task<ResponseProductDTO?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task<IEnumerable<ResponseProductDTO>> GetAllAsync() => _repo.GetAllAsync();

    public async Task<ResponseProductDTO?> AddAsync(ProductCreateDto dto, IEnumerable<IFormFile>? files = null, IFormFile? modelFile = null)
    {
        dto.createDate ??= DateTime.UtcNow;

        var product = await _repo.AddAsync(dto);

        if (files != null)
        {
            foreach (var file in files)
            {
                if (file == null || file.Length == 0) continue;
                await _imageService.UploadProductImageAsync(product.id, file);
            }
        }

        if (modelFile != null && modelFile.Length > 0)
        {
            var modelUrl = await _modelService.UploadModelFileAsync(modelFile);
            if (!string.IsNullOrEmpty(modelUrl) && dto.modelId.HasValue)
            {
                var modelUpdateDto = new ModelUpdateDto
                {
                    id = dto.modelId.Value,
                    name = Path.GetFileNameWithoutExtension(modelFile.FileName),
                    address = modelUrl
                };
                await _modelService.UpdateAsync(modelUpdateDto);
            }
        }

        return await _repo.GetByIdAsync(product.id);
    }

    public Task<int> UpdateAsync(ProductUpdateDto dto) => _repo.UpdateAsync(dto);
    public Task<int> DeleteAsync(int id) => _repo.DeleteProductAsync(id);
}
