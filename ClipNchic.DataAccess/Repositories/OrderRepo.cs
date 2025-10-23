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

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.Images)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.BlindBox)
                        .ThenInclude(bb => bb.Images)
                .Where(o => o.status != "pending")
                .OrderByDescending(o => o.createDate)
                .ToListAsync();
        }

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

        public async Task<(int OrdersCount, decimal CompletedSalesTotal)> GetTodaysOrdersAndCompletedSalesAsync()
        {
            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);

            var ordersCountTask = _context.Orders
                .CountAsync(o => o.createDate >= today && o.createDate < tomorrow);

            var sumTask = _context.Orders
                .Where(o => o.createDate >= today && o.createDate < tomorrow && o.status == "delivered")
                .SumAsync(o => (decimal?)o.totalPrice);

            await Task.WhenAll(ordersCountTask, sumTask);

            var ordersCount = ordersCountTask.Result;
            var total = sumTask.Result ?? 0m;

            return (ordersCount, total);
        }

        public async Task<MonthlySalesSummaryDto> GetYearlySalesSummaryAsync(int year)
        {
            var monthlyData = await _context.Orders
                .Where(o => o.createDate.Value.Year == year)
                .GroupBy(o => o.createDate.Value.Month)
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

            return topProducts;
        }

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

            return topBlindBoxes;
        }

        //public async Task<Order?> GetCartByUserIdAsync(int userId)
        //{
        //    return await _context.Orders
        //        .Include(o => o.OrderDetails)
        //        .ThenInclude(od => od.Product)
        //        .FirstOrDefaultAsync(o => o.userId == userId && o.status == "Cart");
        //}

        //public async Task<Order> CreateCartAsync(int userId)
        //{
        //    var cart = new Order
        //    {
        //        userId = userId,
        //        status = "Cart",
        //        createDate = DateTime.UtcNow,
        //        totalPrice = 0
        //    };
        //    _context.Orders.Add(cart);
        //    await _context.SaveChangesAsync();
        //    return cart;
        //}

        //public async Task AddOrUpdateCartItemAsync(int cartId, int productId, int quantity, decimal price)
        //{
        //    var detail = await _context.OrderDetails.FirstOrDefaultAsync(od => od.orderId == cartId && od.productId == productId);
        //    if (detail == null)
        //    {
        //        detail = new OrderDetail { orderId = cartId, productId = productId, quantity = quantity, price = price };
        //        _context.OrderDetails.Add(detail);
        //    }
        //    else
        //    {
        //        detail.quantity = quantity;
        //        detail.price = price;
        //    }
        //    await _context.SaveChangesAsync();
        //}

        //public async Task RemoveCartItemAsync(int cartId, int productId)
        //{
        //    var detail = await _context.OrderDetails.FirstOrDefaultAsync(od => od.orderId == cartId && od.productId == productId);
        //    if (detail != null)
        //    {
        //        _context.OrderDetails.Remove(detail);
        //        await _context.SaveChangesAsync();
        //    }
        //}

        //public async Task CheckoutAsync(int cartId, decimal totalPrice)
        //{
        //    var cart = await _context.Orders.FindAsync(cartId);
        //    if (cart != null && cart.status == "Cart")
        //    {
        //        cart.status = "Completed";
        //        cart.totalPrice = totalPrice;
        //        cart.createDate = DateTime.UtcNow;
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}
