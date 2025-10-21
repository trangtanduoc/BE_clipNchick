using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;

namespace ClipNchic.Business.Services
{
    public class OrderService
    {
        private readonly OrderRepo _orderRepo;
        private readonly ProductRepo _productRepo;
        private readonly BlindBoxRepo _blindBoxRepo;
        public OrderService(OrderRepo orderRepo, ProductRepo productRepo, BlindBoxRepo blindBoxRepo)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _blindBoxRepo = blindBoxRepo;
        }

        public Task<List<Order>> GetAllOrdersAsync()
        {
            return _orderRepo.GetAllOrdersAsync();
        }
        // Lấy tất cả OrderDetail theo OrderId của order pending
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
            if(order != null)
            {
                foreach (var detail in order.OrderDetails)
                {
                    var product = detail.Product;
                    var firstImage = product?.Images?.FirstOrDefault();
                }
            }
            return order;
        }
        public Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return _orderRepo.GetOrdersByUserIdAsync(userId);
        }

        // Thêm OrderDetail
        public async Task<Order> AddOrderDetailAsync(int userId, string? phone, string? address, string? name, int productId, int quantity, decimal price)
        {
            var order = await _orderRepo.GetPendingOrderByUserIdAsync(userId)
                        ?? await GetOrCreatePendingOrderAsync(userId, phone, address, name);

            var existingDetail = await _orderRepo.GetOrderDetailByOrderAndProductAsync(order.id, productId);
            var product = await _productRepo.GetByIdAsync(productId);

            if (existingDetail != null)
            {
                // Nếu đã có thì cộng thêm số lượng mới
                existingDetail.quantity = (existingDetail.quantity ?? 0) + quantity;
                existingDetail.price = product.Totalprice * existingDetail.quantity;
                await _orderRepo.UpdateOrderDetailAsync(existingDetail);
            }
            else
            {
                // Nếu chưa có thì tạo mới
                var detail = new OrderDetail
                {
                    orderId = order.id,
                    productId = productId,
                    quantity = quantity,
                    price = product.Totalprice * quantity
                };

                await _orderRepo.AddOrderDetailAsync(detail);
            }

            // cập nhật giá
            order.totalPrice = (order.totalPrice ?? 0) + price;
            order.shipPrice = 30000;
            order.payPrice = order.totalPrice + order.shipPrice;

            await _orderRepo.UpdateOrderAsync(order);
            return order;
        }

        public async Task<Order> AddBlindBoxDetailAsync(int userId, string? phone, string? address, string? name, int blindBoxId, int quantity, decimal price)
        {
            var order = await _orderRepo.GetPendingOrderByUserIdAsync(userId)
                        ?? await GetOrCreatePendingOrderAsync(userId, phone, address, name);

            var existingDetail = await _orderRepo.GetOrderDetailByOrderAndBlindBoxAsync(order.id, blindBoxId);
            var blindBox = await _blindBoxRepo.GetByIdAsync(blindBoxId);

            if (existingDetail != null)
            {
                existingDetail.quantity = (existingDetail.quantity ?? 0) + quantity;
                existingDetail.price = blindBox.price * existingDetail.quantity;
                await _orderRepo.UpdateOrderDetailAsync(existingDetail);
            }
            else
            {
                var detail = new OrderDetail
                {
                    orderId = order.id,
                    blindBoxId = blindBoxId,
                    quantity = quantity,
                    price = blindBox.price * quantity
                };

                await _orderRepo.AddOrderDetailAsync(detail);
            }

            order.totalPrice = (order.totalPrice ?? 0) + price;
            order.shipPrice = 30000;
            order.payPrice = order.totalPrice + order.shipPrice;

            await _orderRepo.UpdateOrderAsync(order);
            return order;
        }

        // Xóa OrderDetail
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
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepo.GetOrderByIdAsync(orderId);
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            var orderDetails = await _orderRepo.GetOrderDetailsByOrderIdAsync(orderId);
            foreach (var detail in orderDetails)
            {
                var product = detail.Product;
                var firstImage = product?.Images?.FirstOrDefault();
            }
            return orderDetails;
        }

        public async Task<bool> UpdateOrderAsync(int orderId, OrderDTO dto)
        {
            var existingOrder = await _orderRepo.GetOrderByIdAsync(orderId);
            if (existingOrder == null)
                return false;

            // Cập nhật các trường cho phép thay đổi
            existingOrder.phone = dto.Phone;
            existingOrder.address = dto.Address;
            existingOrder.name = dto.Name;
            existingOrder.status = dto.Status;
            existingOrder.payMethod = dto.PayMethod;
            existingOrder.totalPrice = dto.TotalPrice;
            existingOrder.shipPrice = dto.ShipPrice;
            existingOrder.payPrice = dto.PayPrice;

            await _orderRepo.UpdateOrderAsync(existingOrder);
            return true;
        }

        public async Task<bool> UpdateOrderDetailAsync(int orderDetailId, int quantity)
        {
            var existingDetail = await _orderRepo.GetOrderDetailByIdAsync(orderDetailId);
            var product = await _productRepo.GetByIdAsync(existingDetail?.productId ?? 0);
            var order = await _orderRepo.GetOrderByIdAsync(existingDetail?.orderId ?? 0);
            if (existingDetail == null)
                return false;

            existingDetail.quantity = quantity;
            order.totalPrice = order.totalPrice - existingDetail.price + (product.Totalprice * quantity);
            existingDetail.price = product.Totalprice * quantity;
            order.payPrice = order.totalPrice + order.shipPrice;

            await _orderRepo.UpdateOrderAsync(order);
            await _orderRepo.UpdateOrderDetailAsync(existingDetail);
            return true;
        }

        // Cập nhật status
        public async Task<bool> UpdateStatusAsync(int orderId, string newStatus)
        {
            var order = await _orderRepo.GetOrderByIdAsync(orderId);
            if (order == null) return false;

            order.status = newStatus;
            await _orderRepo.UpdateOrderAsync(order);
            return true;
        }

        // Cập nhật payMethod
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
