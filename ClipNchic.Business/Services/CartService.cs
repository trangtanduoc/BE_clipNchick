using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Repositories;

namespace ClipNchic.Business.Services
{
    public class CartService
    {
        private readonly CartRepo _cartRepo;
        public CartService(CartRepo cartRepo)
        {
            _cartRepo = cartRepo;
        }

        public async Task<Order?> GetCartAsync(int userId)
            => await _cartRepo.GetCartByUserIdAsync(userId);

        public async Task<Order> CreateCartAsync(int userId)
            => await _cartRepo.CreateCartAsync(userId);

        public async Task AddOrUpdateCartItemAsync(int cartId, int designId, int quantity, decimal price)
            => await _cartRepo.AddOrUpdateCartItemAsync(cartId, designId, quantity, price);

        public async Task RemoveCartItemAsync(int cartId, int designId)
            => await _cartRepo.RemoveCartItemAsync(cartId, designId);

        public async Task CheckoutAsync(int cartId, decimal totalAmount)
            => await _cartRepo.CheckoutAsync(cartId, totalAmount);
    }
}
