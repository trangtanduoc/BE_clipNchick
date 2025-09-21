﻿using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Repositories;

namespace ClipNchic.Business.Services
{
    public class OrderService
    {
        private readonly OrderRepo _repo;

        public OrderService(OrderRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync() => await _repo.GetAllAsync();
        public async Task<Order?> GetOrderAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<int> CreateOrderAsync(Order order)
        {
            order.createDate = DateTime.UtcNow;
            return await _repo.CreateAsync(order);
        }
        public async Task<int> UpdateOrderAsync(Order order) => await _repo.UpdateAsync(order);
        public async Task<int> DeleteOrderAsync(int id) => await _repo.DeleteAsync(id);
    }
}
