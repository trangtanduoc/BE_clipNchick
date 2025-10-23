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
            if (order != null)
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

            if (product == null)
            {
                throw new InvalidOperationException($"Product with ID {productId} not found.");
            }

            if (existingDetail != null)
            {
                // Nếu đã có thì cộng thêm số lượng mới
                existingDetail.quantity = (existingDetail.quantity ?? 0) + quantity;
                if (existingDetail.quantity >= product.stock)
                {
                    existingDetail.quantity = product.stock;
                }
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
                if (detail.quantity >= product.stock)
                {
                    detail.quantity = product.stock;
                }

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

            if (blindBox == null)
            {
                throw new InvalidOperationException($"BlindBox with ID {blindBoxId} not found.");
            }

            if (existingDetail != null)
            {
                existingDetail.quantity = (existingDetail.quantity ?? 0) + quantity;
                if (existingDetail.quantity >= blindBox.stock)
                {
                    existingDetail.quantity = blindBox.stock;
                }
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
                if (detail.quantity >= blindBox.stock)
                {
                    detail.quantity = blindBox.stock;
                }

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
            {
                existingOrder.status = dto.Status;
                if (dto.Status == "payment")
                {
                    foreach (var detail in existingOrder.OrderDetails)
                    {
                        var productDetail = detail.Product;
                        var blindBoxDetail = detail.BlindBox;
                        var product = _productRepo.GetByIdAsync(productDetail?.id ?? 0).Result;
                        var blindBox = _blindBoxRepo.GetByIdAsync(blindBoxDetail?.id ?? 0).Result;
                        if (product != null && product.stock.HasValue && detail.quantity.HasValue)
                        {
                            if (product.stock.Value <= detail.quantity.Value)
                            {
                                await _productRepo.updateStock(product.id, 0);
                            }
                            else
                            {
                                await _productRepo.updateStock(product.id, product.stock.Value - detail.quantity.Value);
                            }
                        }
                        if (blindBox != null && blindBox.stock.HasValue && detail.quantity.HasValue)
                        {
                            if (blindBox.stock.Value <= detail.quantity.Value)
                            {
                                await _blindBoxRepo.updateStock(blindBox.id, 0);
                            }
                            else
                            {
                                await _blindBoxRepo.updateStock(blindBox.id, blindBox.stock.Value - detail.quantity.Value);
                            }
                        }
                    }
                }
                if (dto.Status == "cancelled" || dto.Status == "returned")
                {
                    foreach (var detail in existingOrder.OrderDetails)
                    {
                        var productDetail = detail.Product;
                        var blindBoxDetail = detail.BlindBox;
                        var product = _productRepo.GetByIdAsync(productDetail?.id ?? 0).Result;
                        var blindBox = _blindBoxRepo.GetByIdAsync(blindBoxDetail?.id ?? 0).Result;
                        if (product != null && product.stock.HasValue && detail.quantity.HasValue)
                        {
                            await _productRepo.updateStock(product.id, product.stock.Value + detail.quantity.Value);
                        }
                        if (blindBox != null && blindBox.stock.HasValue && detail.quantity.HasValue)
                        {
                            await _blindBoxRepo.updateStock(blindBox.id, blindBox.stock.Value + detail.quantity.Value);
                        }
                    }
                }
            }

            if (dto.PayMethod != null)
            {
                existingOrder.payMethod = dto.PayMethod;
                existingOrder.createDate = DateTime.Now;
            }

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
            if (existingDetail == null)
                return false;

            existingDetail.quantity = quantity;

            if (product != null)
            {
                var product = await _productRepo.GetByIdAsync(existingDetail.productId.Value);
                if (product != null)
                {
                    if (existingDetail.quantity >= product.stock)
                    {
                        existingDetail.quantity = product.stock;
                    }
                    existingDetail.price = product.Totalprice * quantity;
                }
            }
            
            if (existingDetail.blindBoxId.HasValue)
            {
                var blindbox = await _blindBoxRepo.GetByIdAsync(existingDetail.blindBoxId.Value);
                if (blindbox != null)
                {
                    if (existingDetail.quantity >= blindbox.stock)
                    {
                        existingDetail.quantity = blindbox.stock;
                    }
                    existingDetail.price = blindbox.price * quantity;
                }
            }

            await _orderRepo.UpdateOrderDetailAsync(existingDetail);
            return true;
        }

        // Cập nhật status
        public async Task<bool> UpdateStatusAsync(int orderId, string newStatus)
        {
            var order = await _orderRepo.GetOrderByIdAsync(orderId);
            if (order == null) return false;
            if (newStatus == "payment")
            {
                foreach (var detail in order.OrderDetails)
                {
                    var productDetail = detail.Product;
                    var blindBoxDetail = detail.BlindBox;
                    var product = _productRepo.GetByIdAsync(productDetail?.id ?? 0).Result;
                    var blindBox = _blindBoxRepo.GetByIdAsync(blindBoxDetail?.id ?? 0).Result;
                    if (product != null && product.stock.HasValue && detail.quantity.HasValue)
                    {
                        if (product.stock.Value <= detail.quantity.Value)
                        {
                            await _productRepo.updateStock(product.id, 0);
                        }
                        else
                        {
                            await _productRepo.updateStock(product.id, product.stock.Value - detail.quantity.Value);
                        }
                    }
                    if (blindBox != null && blindBox.stock.HasValue && detail.quantity.HasValue)
                    {
                        if (blindBox.stock.Value <= detail.quantity.Value)
                        {
                            await _blindBoxRepo.updateStock(blindBox.id, 0);
                        }
                        else
                        {
                            await _blindBoxRepo.updateStock(blindBox.id, blindBox.stock.Value - detail.quantity.Value);
                        }
                    }
                }
            }
            if (newStatus == "cancelled" || newStatus == "returned")
            {
                foreach (var detail in order.OrderDetails)
                {
                    var productDetail = detail.Product;
                    var blindBoxDetail = detail.BlindBox;
                    var product = _productRepo.GetByIdAsync(productDetail?.id ?? 0).Result;
                    var blindBox = _blindBoxRepo.GetByIdAsync(blindBoxDetail?.id ?? 0).Result;
                    if (product != null && product.stock.HasValue && detail.quantity.HasValue)
                    {
                        await _productRepo.updateStock(product.id, product.stock.Value + detail.quantity.Value);
                    }
                    if (blindBox != null && blindBox.stock.HasValue && detail.quantity.HasValue)
                    {
                        await _blindBoxRepo.updateStock(blindBox.id, blindBox.stock.Value + detail.quantity.Value);
                    }
                }
            }

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
        public async Task<MonthlySalesSummaryDto> GetYearlySalesSummaryAsync(int year)
        {
            return await _orderRepo.GetYearlySalesSummaryAsync(year);
        }

        public async Task<(int OrdersCount, decimal CompletedSalesTotal)> GetTodaysOrdersAndCompletedSalesAsync()
        {
            return await _orderRepo.GetTodaysOrdersAndCompletedSalesAsync();
        }

        public async Task<List<TopSalesDto>> GetTop10ProductsLast30DaysAsync()
        {
            return await _orderRepo.GetTop10ProductsLast30DaysAsync();
        }

        public async Task<List<TopSalesDto>> GetTop10BlindBoxesLast30DaysAsync()
        {
            return await _orderRepo.GetTop10BlindBoxesLast30DaysAsync();
        }
    }
}
