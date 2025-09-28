using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClipNchic.Api.Controllers
{
    [ApiController]
    [Route("Order")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _service;

        public OrderController(OrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _service.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var order = await _service.GetOrderAsync(id);
            if (order == null) return NotFound(new { message = "Order not found" });
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            var result = await _service.CreateOrderAsync(order);
            if (result > 0) return Ok(new { message = "Order created successfully" });
            return BadRequest(new { message = "Failed to create order" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Order order)
        {
            if (id != order.id) return BadRequest(new { message = "Order ID mismatch" });

            var result = await _service.UpdateOrderAsync(order);
            if (result > 0) return Ok(new { message = "Order updated successfully" });
            return BadRequest(new { message = "Failed to update order" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteOrderAsync(id);
            if (result > 0) return Ok(new { message = "Order deleted successfully" });
            return NotFound(new { message = "Order not found" });
        }
    }
}
