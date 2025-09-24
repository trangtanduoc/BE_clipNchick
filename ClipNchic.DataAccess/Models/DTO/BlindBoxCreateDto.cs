namespace ClipNchic.DataAccess.Models.DTO
{
    public class BlindBoxCreateDto
    {
        public int? collectId { get; set; }
        public string? name { get; set; }
        public string? descript { get; set; }
        public decimal? price { get; set; }
        public int? stock { get; set; }
        public string? status { get; set; }
    }
}