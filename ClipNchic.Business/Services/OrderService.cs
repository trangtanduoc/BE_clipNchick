using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Repositories;

namespace ClipNchic.Business.Services
{
    public class OrderService
    {
        private readonly OrderRepo _orderRepo;
        public OrderService(OrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }
        // 1️⃣ Lấy tất cả OrderDetail theo OrderId của order pending
        public async Task<Order> GetOrCreatePendingOrderAsync(int userId, string? phone, string? address, string? name)
        {
            var order = await _orderRepo.GetPendingOrderByUserIdAsync(userId);
            if (order == null)
            {
                order = new Order
                {
                    userId = userId,
                    phone = phone,
                    address = address,
                    name = name,
                    createDate = DateTime.Now,
                    status = "pending",
                    shipPrice = 0,
                    totalPrice = 0,
                    payPrice = 0
                };
                await _orderRepo.CreatePendingOrderAsync(order);
            }
            return order;
        }

        // 2️⃣ Thêm OrderDetail
        public async Task<Order> AddOrderDetailAsync(int userId, string? phone, string? address, string? name, int productId, int quantity, decimal price)
        {
            var order = await _orderRepo.GetPendingOrderByUserIdAsync(userId)
                        ?? await GetOrCreatePendingOrderAsync(userId, phone, address, name);

            var detail = new OrderDetail
            {
                orderId = order.id,
                productId = productId,
                quantity = quantity,
                price = price
            };

            await _orderRepo.AddOrderDetailAsync(detail);

            // cập nhật giá
            order.totalPrice = (order.totalPrice ?? 0) + quantity * price;
            order.shipPrice = 30000;
            order.payPrice = order.totalPrice + order.shipPrice;

            await _orderRepo.UpdateOrderAsync(order);
            return order;
        }

        // 3️⃣ Xóa OrderDetail
        public async Task<Order?> DeleteOrderDetailAsync(int userId, int orderDetailId)
        {
            var order = await _orderRepo.GetPendingOrderByUserIdAsync(userId);
            if (order == null) return null;

            var detail = order.OrderDetails.FirstOrDefault(d => d.id == orderDetailId);
            if (detail == null) return order;

            await _orderRepo.DeleteOrderDetailAsync(detail);

            // cập nhật giá
            order.totalPrice = (order.totalPrice ?? 0) - (detail.quantity ?? 0) * (detail.price ?? 0);
            if (order.totalPrice <= 0)
            {
                order.totalPrice = 0;
                order.shipPrice = 0;
            }
            order.payPrice = order.totalPrice + order.shipPrice;

            await _orderRepo.UpdateOrderAsync(order);
            return order;
        }

        // 4️⃣ Cập nhật status
        public async Task<bool> UpdateStatusAsync(int orderId, string newStatus)
        {
            var order = await _orderRepo.GetOrderByIdAsync(orderId);
            if (order == null) return false;

            order.status = newStatus;
            await _orderRepo.UpdateOrderAsync(order);
            return true;
        }

        // 5️⃣ Cập nhật payMethod
        public async Task<bool> UpdatePayMethodAsync(int orderId, string method)
        {
            var order = await _orderRepo.GetOrderByIdAsync(orderId);
            if (order == null) return false;

            order.payMethod = method;
            order.createDate = DateTime.Now;
            await _orderRepo.UpdateOrderAsync(order);
            return true;
        }
    }
}
