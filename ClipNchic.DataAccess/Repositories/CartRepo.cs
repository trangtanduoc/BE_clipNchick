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
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.userId == userId && o.status == "Cart");
        }

        public async Task<Order> CreateCartAsync(int userId)
        {
            var cart = new Order
            {
                userId = userId,
                status = "Cart",
                createDate = DateTime.UtcNow,
                totalPrice = 0
            };
            _context.Orders.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task AddOrUpdateCartItemAsync(int cartId, int productId, int quantity, decimal price)
        {
            var detail = await _context.OrderDetails.FirstOrDefaultAsync(od => od.orderId == cartId && od.productId == productId);
            if (detail == null)
            {
                detail = new OrderDetail { orderId = cartId, productId = productId, quantity = quantity, price = price };
                _context.OrderDetails.Add(detail);
            }
            else
            {
                detail.quantity = quantity;
                detail.price = price;
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCartItemAsync(int cartId, int productId)
        {
            var detail = await _context.OrderDetails.FirstOrDefaultAsync(od => od.orderId == cartId && od.productId == productId);
            if (detail != null)
            {
                _context.OrderDetails.Remove(detail);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CheckoutAsync(int cartId, decimal totalPrice)
        {
            var cart = await _context.Orders.FindAsync(cartId);
            if (cart != null && cart.status == "Cart")
            {
                cart.status = "Completed";
                cart.totalPrice = totalPrice;
                cart.createDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
