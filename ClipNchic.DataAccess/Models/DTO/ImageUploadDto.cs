using Microsoft.AspNetCore.Http;

namespace ClipNchic.DataAccess.Models.DTo
{
    public class ImageUploadDto
    {
        public int id { get; set; }
        public required IFormFile file { get; set; }
    }
}