namespace ClipNchic.DataAccess.Models.DTO
{
    public class BaseUpdateDto
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? color { get; set; }
        public decimal? price { get; set; }
        public int? modelId { get; set; }
    }
}