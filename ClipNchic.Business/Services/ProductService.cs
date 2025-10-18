
﻿﻿using ClipNchic.DataAccess.Models;
using ClipNchic.DataAccess.Models.DTO;
using ClipNchic.DataAccess.Repositories;

namespace ClipNchic.Business.Services
{
    public class ProductService
    {
        private readonly ProductRepo _repo;
        public ProductService(ProductRepo repo) => _repo = repo;
        public async Task<ResponseProductDTO?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<IEnumerable<ResponseProductDTO>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<int> AddAsync(ProductCreateDto dto) => await _repo.AddAsync(dto);
        public async Task<int> UpdateAsync(ProductUpdateDto dto) => await _repo.UpdateAsync(dto);
        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);

    }
}