using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ClipNchic.DataAccess.Repositories
{
    public class OrderRepo
    {
        private readonly AppDbContext _context;
        public OrderRepo(AppDbContext context)
        {
            _context = context;
        }

#pragma warning disable CS8602
        public async Task<List<OrderResponseDto>> GetAllOrdersAsync()
        {
            return await ProjectOrders(excludePending: true).ToListAsync();
        }

        public async Task<OrderResponseDto?> GetOrderResponseByIdAsync(int orderId)
        {
            return await ProjectOrders(excludePending: false)
                .FirstOrDefaultAsync(o => o.id == orderId);
        }

        private IQueryable<OrderResponseDto> ProjectOrders(bool excludePending)
        {
            var query = _context.Orders.AsQueryable();
            if (excludePending)
            {
                query = query.Where(o => o.status != "pending");
            }

            return query
                .OrderByDescending(o => o.createDate)
                .Select(o => new OrderResponseDto
                {
                    id = o.id,
                    userId = o.userId,
                    user = o.User == null ? null : new UserLiteDto
                    {
                        id = o.User.id,
                        email = o.User.email,
                        phone = o.User.phone,
                        name = o.User.name,
                        address = o.User.address,
                        image = o.User.image
                    },
                    phone = o.phone,
                    address = o.address,
                    name = o.name,
                    createDate = o.createDate,
                    totalPrice = o.totalPrice,
                    shipPrice = o.shipPrice,
                    payPrice = o.payPrice,
                    status = o.status,
                    payMethod = o.payMethod,
                    orderDetails = o.OrderDetails.Select(od => new OrderDetailResponseDto
                    {
                        id = od.id,
                        productId = od.productId,
                        product = od.Product == null ? null : new ProductLiteDto
                        {
                            id = od.Product.id,
                            title = od.Product.title,
                            descript = od.Product.descript,
                            price = od.Product.price,
                            status = od.Product.status,
                            model = od.Product.Model == null ? null : new ModelLiteDto
                            {
                                address = od.Product.Model.address
                            },
                            images = od.Product.Images
                                .Select(img => new ImageLiteDto
                                {
                                    id = img.id,
                                    name = img.name,
                                    address = img.address
                                })
                                .ToList()
                        },
                        blindBoxId = od.blindBoxId,
                        blindBox = od.BlindBox == null ? null : new BlindBoxLiteDto
                        {
                            id = od.BlindBox.id,
                            name = od.BlindBox.name,
                            descript = od.BlindBox.descript,
                            price = od.BlindBox.price,
                            images = od.BlindBox.Images
                                .Select(img => new ImageLiteDto
                                {
                                    id = img.id,
                                    name = img.name,
                                    address = img.address
                                })
                                .ToList()
                        },
                        quantity = od.quantity,
                        price = od.price
                    }).ToList()
                });
        }
#pragma warning restore CS8602

#pragma warning disable CS8602
        public async Task<Order?> GetPendingOrderByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                      .ThenInclude(p => p.Images)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.BlindBox)
                        .ThenInclude(bb => bb.Images)
                .FirstOrDefaultAsync(o => o.userId == userId && o.status == "pending");
        }
        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.Images)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.BlindBox)
                        .ThenInclude(bb => bb.Images)
                .Where(o => o.userId == userId && o.status != "pending")
                .OrderByDescending(o => o.createDate)
                .ToListAsync();
        }
#pragma warning restore CS8602

        public async Task<Order> CreatePendingOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task AddOrderDetailAsync(OrderDetail detail)
        {
            _context.OrderDetails.Add(detail);
            await _context.SaveChangesAsync();
        }

        public async Task<OrderDetail?> GetOrderDetailByOrderAndProductAsync(int orderId, int productId)
        {
            return await _context.OrderDetails
                .FirstOrDefaultAsync(d => d.orderId == orderId && d.productId == productId);
        }

        public async Task<OrderDetail?> GetOrderDetailByOrderAndBlindBoxAsync(int orderId, int blindBoxId)
        {
            return await _context.OrderDetails
                .FirstOrDefaultAsync(d => d.orderId == orderId && d.blindBoxId == blindBoxId);
        }

        

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<OrderDetail?> GetOrderDetailByIdAsync(int id)
        {
            return await _context.OrderDetails.FindAsync(id);
        }

        public async Task UpdateOrderDetailAsync(OrderDetail detail)
        {
            _context.OrderDetails.Update(detail);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteOrderDetailAsync(OrderDetail detail)
        {
            _context.OrderDetails.Remove(detail);
            await _context.SaveChangesAsync();
        }

#pragma warning disable CS8602
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.Images)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.BlindBox)
                        .ThenInclude(bb => bb.Images)
                .OrderByDescending(o => o.createDate)
                .FirstOrDefaultAsync(o => o.id == orderId);
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails
                .Include(od => od.Product)
                    .ThenInclude(p => p.Images)
                .Include(od => od.BlindBox)
                    .ThenInclude(bb => bb.Images)
                .Where(od => od.orderId == orderId)
                .ToListAsync();
        }
#pragma warning restore CS8602

        public async Task<(int OrdersCount, decimal CompletedSalesTotal)> GetTodaysOrdersAndCompletedSalesAsync()
        {
            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);

            var ordersCount =await _context.Orders
                .CountAsync(o => o.createDate >= today && o.createDate < tomorrow);

            var sumTask =await _context.Orders
                .Where(o => o.createDate >= today && o.createDate < tomorrow && o.status == "delivered")
                .SumAsync(o => (decimal?)o.totalPrice);

           
            var total = sumTask ?? 0m;  

            return (ordersCount, total);
        }

        public async Task<MonthlySalesSummaryDto> GetYearlySalesSummaryAsync(int year)
        {
            var monthlyData = await _context.Orders
                .Where(o => o.createDate.HasValue && o.createDate.Value.Year == year)
                .GroupBy(o => o.createDate!.Value.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    OrdersCount = g.Count(),
                    SalesTotal = g.Sum(o => (decimal?)o.totalPrice) ?? 0m
                })
                .ToListAsync();

            var summary = new MonthlySalesSummaryDto
            {
                Year = year,
                MonthlySales = Enumerable.Range(1, 12)
                    .Select(m =>
                    {
                        var data = monthlyData.FirstOrDefault(x => x.Month == m);
                        return new MonthlySalesDto
                        {
                            Month = m,
                            OrdersCount = data?.OrdersCount ?? 0,
                            SalesTotal = data?.SalesTotal ?? 0m
                        };
                    })
                    .ToList()
            };

            summary.YearlyTotalOrders = summary.MonthlySales.Sum(m => m.OrdersCount);
            summary.YearlyTotalSales = summary.MonthlySales.Sum(m => m.SalesTotal);

            return summary;

}
#pragma warning disable CS8602
        public async Task<List<TopSalesDto>> GetTop10ProductsLast30DaysAsync()
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            var topProducts = await _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Product)
                .Where(od => od.productId != null && od.Order != null && od.Order.createDate >= thirtyDaysAgo && od.Order.status == "delivered")
                .GroupBy(od => new { od.productId, od.Product!.title })
                .OrderByDescending(g => g.Sum(od => od.quantity ?? 0))
                .Take(10)
                .Select(g => new TopSalesDto
                {
                    id = g.Key.productId ?? 0,
                    name = g.Key.title,
                    quantitySold = g.Sum(od => od.quantity ?? 0)
                })
                .ToListAsync();
            foreach (var item in topProducts)
            {
                var images = await _context.Images
                    .Where(img => img.productId == item.id)
                    .ToListAsync();
                item.Images = images;
            }

            return topProducts;
        }
#pragma warning restore CS8602

#pragma warning disable CS8602
        public async Task<List<TopSalesDto>> GetTop10BlindBoxesLast30DaysAsync()
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            var topBlindBoxes = await _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.BlindBox)
                .Where(od => od.blindBoxId != null && od.Order != null && od.Order.createDate >= thirtyDaysAgo && od.Order.status == "delivered")
                .GroupBy(od => new { od.blindBoxId, od.BlindBox!.name })
                .OrderByDescending(g => g.Sum(od => od.quantity ?? 0))
                .Take(10)
                .Select(g => new TopSalesDto
                {
                    id = g.Key.blindBoxId ?? 0,
                    name = g.Key.name,
                    quantitySold = g.Sum(od => od.quantity ?? 0)
                })
                .ToListAsync();
            foreach (var item in topBlindBoxes)
            {
                var images = await _context.Images
                    .Where(img => img.blindBoxId == item.id)
                    .ToListAsync();
                item.Images = images;
            }

            return topBlindBoxes;
        }
#pragma warning restore CS8602

        public async Task<DailySalesSummaryDto> GetDaily()
        {
            var today = DateTime.UtcNow.Date;
            var ordersToday = await _context.Orders
                .Where(o => o.createDate.HasValue && o.createDate >= today && o.createDate < today.AddDays(1) && o.status != "pending" && o.status != "unknown")
                .ToListAsync();
            var canceledOrdersToday = ordersToday.Where(o => o.status == "cancelled").ToList();
            var summary = new DailySalesSummaryDto
            {
                countOrder = ordersToday.Count,
                totalSales = ordersToday.Where(o => o.status != "pending" && o.status != "unknown").Sum(o => o.totalPrice) ?? 0,
                countOrderCancel = canceledOrdersToday.Count
            };
            return summary;
        }

        public async Task<MonthlySalesOrderDto> GetMonthly()
        {
            var now = DateTime.UtcNow.Date;
            var OrderThisMonth = await _context.Orders
                .Where(o => o.createDate.HasValue && o.createDate.Value.Year == now.Year && o.createDate.Value.Month == now.Month && o.status != "pending" && o.status != "unknown")
                .ToListAsync();
            var lastMonth = now.AddMonths(-1);
            var OrderLastMonth = await _context.Orders
                .Where(o => o.createDate.HasValue && o.createDate.Value.Year == lastMonth.Year && o.createDate.Value.Month == lastMonth.Month && o.status != "pending" && o.status != "unknown")
                .ToListAsync();
            var OrderFailedThisMonth = OrderThisMonth.Where(o => o.status == "cancelled").ToList();
            var OrderFailedLastMonth = OrderLastMonth.Where(o => o.status == "cancelled").ToList();
            var summary = new MonthlySalesOrderDto
            {
                OrderThisMonth = OrderThisMonth.Count,
                ThisMonthSales = OrderThisMonth.Where(o => o.status != "pending" && o.status != "unknown").Sum(o => o.totalPrice) ?? 0,
                OrderLastMonth = OrderLastMonth.Count,
                LastMonthSales = OrderLastMonth.Where(o => o.status != "pending" && o.status != "unknown").Sum(o => o.totalPrice) ?? 0,
                OrderFailedThisMonth = OrderFailedThisMonth.Count,
                OrderFailedLastMonth = OrderFailedLastMonth.Count
            };
            return summary;
        }
    }
}
