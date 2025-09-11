using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ClipNchic.DataAccess.Repositories
{
    public class CartRepo
    {
        private readonly AppDbContext _context;
        public CartRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetCartByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Design)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == "Cart");
        }

        public async Task<Order> CreateCartAsync(int userId)
        {
            var cart = new Order
            {
                UserId = userId,
                Status = "Cart",
                OrderDate = DateTime.UtcNow,
                TotalAmount = 0
            };
            _context.Orders.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task AddOrUpdateCartItemAsync(int cartId, int designId, int quantity, decimal price)
        {
            var detail = await _context.OrderDetails.FirstOrDefaultAsync(od => od.OrderId == cartId && od.DesignId == designId);
            if (detail == null)
            {
                detail = new OrderDetail { OrderId = cartId, DesignId = designId, Quantity = quantity, Price = price };
                _context.OrderDetails.Add(detail);
            }
            else
            {
                detail.Quantity = quantity;
                detail.Price = price;
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCartItemAsync(int cartId, int designId)
        {
            var detail = await _context.OrderDetails.FirstOrDefaultAsync(od => od.OrderId == cartId && od.DesignId == designId);
            if (detail != null)
            {
                _context.OrderDetails.Remove(detail);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CheckoutAsync(int cartId, decimal totalAmount)
        {
            var cart = await _context.Orders.FindAsync(cartId);
            if (cart != null && cart.Status == "Cart")
            {
                cart.Status = "Completed";
                cart.TotalAmount = totalAmount;
                cart.OrderDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
