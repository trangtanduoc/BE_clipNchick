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
        public async Task<Model> AddAsync(ModelCreateDto dto) => await _repo.AddAsync(dto);
        public async Task<int> UpdateAsync(ModelUpdateDto dto) => await _repo.UpdateAsync(dto);
        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public async Task<Model?> CreateModelFromFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            if (!ValidateGlbFile(file))
                throw new InvalidOperationException("Invalid file format. Only GLB files are accepted.");

            var tempPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".glb");
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
                    var createdModel = await _repo.AddAsync(modelDto);
                    return createdModel;
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

        public async Task<Model?> CreateModelFromJsonFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            if (!ValidateJsonFile(file))
                throw new InvalidOperationException("Invalid file format. Only JSON files are accepted.");

            var tempPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".json");
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
                    var createdModel = await _repo.AddAsync(modelDto);
                    return createdModel;
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

        private bool ValidateGlbFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".glb")
                return false;

            var allowedMimeTypes = new[] { "model/gltf-binary", "application/octet-stream" };
            if (!allowedMimeTypes.Contains(file.ContentType))
                return false;

            return true;
        }

        private bool ValidateJsonFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".json")
                return false;

            var allowedMimeTypes = new[] { "application/json" };
            if (!allowedMimeTypes.Contains(file.ContentType))
                return false;

            return true;
        }
    }
}