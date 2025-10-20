using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClipNchic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _service;
        public OrderController(OrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _service.GetAllOrdersAsync();
            return Ok(orders);
        }
        // Lấy thông tin user từ token
        private (int userId, string? name, string? phone, string? address) GetUserInfo()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var phone = User.FindFirst(ClaimTypes.MobilePhone)?.Value;
            var address = User.FindFirst(ClaimTypes.StreetAddress)?.Value;
            return (userId, name, phone, address);
        }


        // Lấy hoặc tạo order pending
        [HttpGet("pending/{userId}")]
        public async Task<IActionResult> GetPendingOrder()
        {
            var (userId, name, phone, address) = GetUserInfo();
            var order = await _service.GetOrCreatePendingOrderAsync(userId, phone, address, name);
            return Ok(order);
        }

        // Thêm orderdetail
        [HttpPost("add-detail")]
        public async Task<IActionResult> AddOrderDetail(int productId, int quantity, decimal price)
        {
            var (userId, name, phone, address) = GetUserInfo();
            var order = await _service.AddOrderDetailAsync(userId, name, phone, address, productId, quantity, price);
            return Ok(order);
        }

        [HttpPost("add-blindbox-detail")]
        public async Task<IActionResult> AddBlindBoxDetail(int blindBoxId, int quantity, decimal price)
        {
            var (userId, name, phone, address) = GetUserInfo();
            var order = await _service.AddBlindBoxDetailAsync(userId, name, phone, address, blindBoxId, quantity, price);
            return Ok(order);
        }

        // Xóa orderdetail
        [HttpDelete("delete-detail/{userId}/{orderDetailId}")]
        public async Task<IActionResult> DeleteOrderDetail(int orderDetailId)
        {
            var (userId, _, _, _) = GetUserInfo();
            var order = await _service.DeleteOrderDetailAsync(userId, orderDetailId);
            return order == null ? NotFound() : Ok(order);
        }

        // Cập nhật status
        [HttpPut("update-status/{orderId}")]
        public async Task<IActionResult> UpdateStatus(int orderId, [FromQuery] string status)
        {
            var result = await _service.UpdateStatusAsync(orderId, status);
            return result ? Ok("Updated") : NotFound();
        }

        // Cập nhật payMethod
        [HttpPut("update-paymethod/{orderId}")]
        public async Task<IActionResult> UpdatePayMethod(int orderId, [FromQuery] string method)
        {
            var result = await _service.UpdatePayMethodAsync(orderId, method);
            return result ? Ok("Updated") : NotFound();
        }

    }

    // DTO cho request thêm orderdetail
    public class OrderDetailRequest
    {
        public int userId { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        public string? name { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
    }
}
