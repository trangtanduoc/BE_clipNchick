using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
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
                .OrderByDescending(o => o.createDate)
                .ToListAsync();
        }

        public async Task<Order?> GetPendingOrderByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.BlindBox)
                .FirstOrDefaultAsync(o => o.userId == userId && o.status == "pending");
        }
        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.Images)
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
                .OrderByDescending(o => o.createDate)
                .FirstOrDefaultAsync(o => o.id == orderId);
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails
                .Include(od => od.Product)
                    .ThenInclude(p => p.Images)
                .Where(od => od.orderId == orderId)
                .ToListAsync();
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
