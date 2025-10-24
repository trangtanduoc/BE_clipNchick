namespace ClipNchic.DataAccess.Repositories
{
    public class DailySalesSummaryDto
    {
        public int? countOrder { get; set; }
        public decimal? totalSales { get; set; }
        public int? countOrderCancel { get; set; }
    }
}