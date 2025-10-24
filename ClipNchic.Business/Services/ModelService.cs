using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace ClipNchic.Business.Services
{
    public class ModelService
    {
        private readonly ModelRepo _repo;
        private readonly Cloudinary _cloudinary;

        public ModelService(ModelRepo repo, Cloudinary cloudinary)
        {
            _repo = repo;
            _cloudinary = cloudinary;
        }

        public async Task<Model?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<IEnumerable<Model>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<int> AddAsync(ModelCreateDto dto) => await _repo.AddAsync(dto);
        public async Task<int> UpdateAsync(ModelUpdateDto dto) => await _repo.UpdateAsync(dto);
        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public async Task<Model?> CreateModelFromFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            var tempPath = Path.GetTempFileName();
            await using (var stream = System.IO.File.Create(tempPath))
            {
                await file.CopyToAsync(stream);
            }

            try
            {
                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(tempPath)
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.StatusCode == HttpStatusCode.OK)
                {
                    var modelDto = new ModelCreateDto
                    {
                        name = Path.GetFileNameWithoutExtension(file.FileName),
                        address = uploadResult.SecureUrl.ToString()
                    };
                    await AddAsync(modelDto);
                    var createdModel = await _repo.GetAllAsync();
                    return createdModel.LastOrDefault();
                }

                return null;
            }
            finally
            {
                if (System.IO.File.Exists(tempPath))
                {
                    System.IO.File.Delete(tempPath);
                }
            }
        }
    }
}