public class MonthlySalesSummaryDto
{
    public int Year { get; set; }
    public List<MonthlySalesDto> MonthlySales { get; set; } = new();
    public decimal YearlyTotalSales { get; set; }
    public int YearlyTotalOrders { get; set; }
}

public class MonthlySalesDto
{
    public int Month { get; set; } // 1 = January, 12 = December
    public int OrdersCount { get; set; }
    public decimal SalesTotal { get; set; }
}

public class MonthlySalesOrderDto
{
    public int OrderThisMonth { get; set; }
    public int OrderLastMonth { get; set; }
    public int OrderFailedThisMonth { get; set; }
    public int OrderFailedLastMonth { get; set; }
}