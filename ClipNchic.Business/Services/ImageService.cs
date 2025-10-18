using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace ClipNchic.Business.Services
{
    public class ImageService
    {
        private readonly ImageRepo _repo;
        private readonly Cloudinary _cloudinary;

        public ImageService(ImageRepo repo, Cloudinary cloudinary)
        {
            _repo = repo;
            _cloudinary = cloudinary;
        }

        public async Task<ImageDetailDto?> GetByIdAsync(int id)
        {
            var image = await _repo.GetByIdAsync(id);
            if (image == null) return null;

            return new ImageDetailDto
            {
                id = image.id,
                name = image.name,
                address = image.address,
                charmId = image.charmId,
                baseId = image.baseId,
                blindBoxId = image.blindBoxId,
                productId = image.productId
            };
        }

        public async Task<IEnumerable<ImageDetailDto>> GetAllAsync(int? baseId, int? charmId, int? blindBoxId, int? productId)
        {
            var images = await _repo.GetAllAsync(baseId, charmId, blindBoxId, productId);
            return images.Select(image => new ImageDetailDto
            {
                id = image.id,
                name = image.name,
                address = image.address,
                charmId = image.charmId,
                baseId = image.baseId,
                blindBoxId = image.blindBoxId,
                productId = image.productId
            });
        }

        public async Task<int> AddAsync(ImageCreateDto dto) => await _repo.AddAsync(dto);
        public async Task<int> UpdateAsync(Image image) => await _repo.UpdateAsync(image);
        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public async Task<string?> UpdateImageWithCloudinaryAsync(int id, string localFilePath)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(localFilePath)
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var image = await _repo.GetByIdAsync(id);
                if (image == null) return null;
                image.address = uploadResult.SecureUrl.ToString();
                await _repo.UpdateAsync(image);
                return image.address;
            }
            return null;
        }

        
    }
}