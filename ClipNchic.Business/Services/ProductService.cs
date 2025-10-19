using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;

namespace ClipNchic.Business.Services;

public class ProductService
{
    private readonly ProductRepo _repo;
    private readonly ImageService _imageService;

    public ProductService(ProductRepo repo, ImageService imageService)
    {
        _repo = repo;
        _imageService = imageService;
    }

    public Task<ResponseProductDTO?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task<IEnumerable<ResponseProductDTO>> GetAllAsync() => _repo.GetAllAsync();

    public async Task<ResponseProductDTO?> AddAsync(ProductCreateDto dto, IEnumerable<IFormFile>? files = null)
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

        return await _repo.GetByIdAsync(product.id);
    }

    public Task<int> UpdateAsync(ProductUpdateDto dto) => _repo.UpdateAsync(dto);
    public Task<int> DeleteAsync(int id) => _repo.DeleteProductAsync(id);
}
