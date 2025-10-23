

namespace ClipNchic.DataAccess.Models.DTO
{
    public class ResponseCharmDTO
    {
        public int id { get; set; }
        public string? name { get; set; }
        public decimal? price { get; set; }
        public int? modelId { get; set; }
        public ImageDetailDto? Image { get; set; }
        public ModelDetailDto? Model { get; set; }
        public ICollection<CharmProduct> CharmProducts { get; set; } = new List<CharmProduct>();
    }
}
