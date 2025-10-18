using ClipNchic.DataAccess.Models;
using System.Collections.Generic;

namespace ClipNchic.DataAccess.Models.DTO
{
    public class ResponseBaseDTO
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? color { get; set; }
        public decimal? price { get; set; }
        public int? modelId { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public Model? Model { get; set; }
    }
}