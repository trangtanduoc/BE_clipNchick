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

            if (dto.Phone != null)
                existingOrder.phone = dto.Phone;

            if (dto.Address != null)
                existingOrder.address = dto.Address;

            if (dto.Name != null)
                existingOrder.name = dto.Name;

            if (dto.Status != null)
                existingOrder.status = dto.Status;

            if (dto.PayMethod != null)
                existingOrder.payMethod = dto.PayMethod;

            if (dto.TotalPrice.HasValue)
                existingOrder.totalPrice = dto.TotalPrice.Value;

            if (dto.ShipPrice.HasValue)
                existingOrder.shipPrice = dto.ShipPrice.Value;

            if (dto.PayPrice.HasValue)
                existingOrder.payPrice = dto.PayPrice.Value;

            await _orderRepo.UpdateOrderAsync(existingOrder);
            return true;
        }


        public async Task<bool> UpdateOrderDetailAsync(int orderDetailId, int quantity)
        {
            var existingDetail = await _orderRepo.GetOrderDetailByIdAsync(orderDetailId);
            var product = await _productRepo.GetByIdAsync(existingDetail?.productId ?? 0);
            var blindbox = await _blindBoxRepo.GetByIdAsync(existingDetail?.blindBoxId ?? 0);
            if (existingDetail == null)
                return false;

            existingDetail.quantity = quantity;
            if(product != null)
                existingDetail.price = product.Totalprice * quantity;
            if(blindbox != null)
                existingDetail.price = blindbox.price * quantity;

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
