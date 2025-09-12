using Microsoft.AspNetCore.Mvc;
using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;

namespace ClipNchic.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ActionResult<Order?>> GetCart(int userId)
        {
            var cart = await _cartService.GetCartAsync(userId);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost("CreateCart/{userId}")]
        public async Task<ActionResult<Order>> CreateCart(int userId)
        {
            var cart = await _cartService.CreateCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("AddOrUpdateCartItem/{cartId}")]
        public async Task<IActionResult> AddOrUpdateCartItem(int cartId, [FromBody] CartItemDto dto)
        {
            await _cartService.AddOrUpdateCartItemAsync(cartId, dto.DesignId, dto.Quantity, dto.Price);
            return Ok();
        }

        [HttpDelete("{cartId}/item/{designId}")]
        public async Task<IActionResult> RemoveCartItem(int cartId, int designId)
        {
            await _cartService.RemoveCartItemAsync(cartId, designId);
            return Ok();
        }

        [HttpPost("Checkout/{cartId}")]
        public async Task<IActionResult> Checkout(int cartId, [FromBody] CheckoutDto dto)
        {
            await _cartService.CheckoutAsync(cartId, dto.TotalAmount);
            return Ok();
        }
    }
}
public class CartItemDto
{
    public int DesignId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class CheckoutDto
{
    public decimal TotalAmount { get; set; }
}