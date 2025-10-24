using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid or missing user ID in token");
            }
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var phone = User.FindFirst(ClaimTypes.MobilePhone)?.Value;
            var address = User.FindFirst(ClaimTypes.StreetAddress)?.Value;
            return (userId, name, phone, address);
        }

        // Lấy tất cả order của user
        [HttpGet("user-orders/{userId}")]
        public async Task<IActionResult> GetUserOrders(int userId)
        {
            var (tokenUserId, _, _, _) = GetUserInfo();
            if (tokenUserId != userId)
            {
                return Forbid();
            }
            var orders = await _service.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        // Lấy hoặc tạo order pending
        [HttpGet("pending/{userId}")]
        public async Task<IActionResult> GetPendingOrder(int userId)
        {
            var (tokenUserId, name, phone, address) = GetUserInfo();
            if (tokenUserId != userId)
            {
                return Forbid();
            }
            var order = await _service.GetOrCreatePendingOrderAsync(userId, phone, address, name);
            return Ok(order);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _service.GetOrderByIdAsync(orderId);
            return order == null ? NotFound(new { message = "Not found" }) : Ok(order);
        }

        [HttpGet("detail/{orderDetailId}")]
        public async Task<IActionResult> GetOrderDetailById(int orderDetailId)
        {
            var detail = await _service.GetOrderDetailsByOrderIdAsync(orderDetailId);
            return detail == null ? NotFound(new { message = "Not found" }) : Ok(detail);
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

        [HttpPut("update-order/{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] OrderDTO dto)
        {
            var result = await _service.UpdateOrderAsync(orderId, dto);
            return result ? Ok(new {message = "Updated" }) : NotFound(new { message = "Failed to update" });
        }

        [HttpPut("update-quantity-detail/{orderDetailId}")]
        public async Task<IActionResult> UpdateOrderDetail(int orderDetailId, int quantity)
        {
            var result = await _service.UpdateOrderDetailAsync(orderDetailId, quantity);
            return result ? Ok(new { message = "Updated" }) : NotFound(new { message = "Failed to update" });
        }


        // Xóa orderdetail
        [HttpDelete("delete-detail/{userId}/{orderDetailId}")]
        public async Task<IActionResult> DeleteOrderDetail(int orderDetailId)
        {
            var (userId, _, _, _) = GetUserInfo();
            var order = await _service.DeleteOrderDetailAsync(userId, orderDetailId);
            return order == null ? NotFound(new { message = "Failed to delete" }) : Ok(order);
        }

        // Cập nhật status
        [HttpPut("update-status/{orderId}")]
        public async Task<IActionResult> UpdateStatus(int orderId, [FromQuery] string status)
        {
            var result = await _service.UpdateStatusAsync(orderId, status);
            return result ? Ok(new { message = "Updated" }) : NotFound(new { message = "Failed to update" });
        }

        // Cập nhật payMethod
        [HttpPut("update-paymethod/{orderId}")]
        public async Task<IActionResult> UpdatePayMethod(int orderId, [FromQuery] string method)
        {
            var result = await _service.UpdatePayMethodAsync(orderId, method);
            return result ? Ok(new { message = "Updated" }) : NotFound(new { message = "Failed to update" });
        }

        [HttpGet("YearlySalesSummary")]
        public async Task<ActionResult<MonthlySalesSummaryDto>> GetYearlySalesSummary([FromQuery] int year)
        {
            var result = await _service.GetYearlySalesSummaryAsync(year);
            return Ok(result);
        }
        // Get top 10 products sold in last 30 days
        [HttpGet("sales/top-products")]
        public async Task<IActionResult> GetTop10ProductsLast30Days()
        {
            var topProducts = await _service.GetTop10ProductsLast30DaysAsync();
            return Ok(topProducts);
        }

        // Get top 10 blind boxes sold in last 30 days
        [HttpGet("sales/top-blindboxes")]
        public async Task<IActionResult> GetTop10BlindBoxesLast30Days()
        {
            var topBlindBoxes = await _service.GetTop10BlindBoxesLast30DaysAsync();
            return Ok(topBlindBoxes);
        }

        
        [HttpGet("TodaysOrdersAndCompletedSales")]
        public async Task<ActionResult<object>> GetTodaysOrdersAndCompletedSales()
        {
            var result = await _service.GetTodaysOrdersAndCompletedSalesAsync();
            return Ok(result);
        }
    }
}
