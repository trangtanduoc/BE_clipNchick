namespace ClipNchic.DataAccess.Models.DTO
{
    public class TopSalesDto
    {
        public int id { get; set; }
        public string? name { get; set; }
        public int quantitySold { get; set; }
        public List<Image>? Images { get; set; } = new();
    }
}
