using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceWarehouse.Server.DTOs;
using EcommerceWarehouse.Server.Entities;
using EcommerceWarehouse.Server.Repositories.Interfaces;
using EcommerceWarehouse.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWarehouse.Server.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _repo;
        private readonly IMapper _mapper;
        public ProductService(IGenericRepository<Product> repo, IMapper mapper)
        {
            _repo = repo; _mapper = mapper;
        }
        public async Task<List<ProductDto>> GetAllAsync()
        {
            var all = await _repo.GetAllAsync(p => p.Category);
            return all.AsQueryable().ProjectTo<ProductDto>(_mapper.ConfigurationProvider).ToList();
        }
        public async Task<PagedResult<ProductDto>> QueryAsync(ProductQuery q)
        {
            var list = await _repo.GetAllAsync(p => p.Category);
            var query = list.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q.Search))
            {
                var s = q.Search.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(s) || p.Sku.ToLower().Contains(s));
            }
            if (q.CategoryId.HasValue) query = query.Where(p => p.CategoryId == q.CategoryId);
            query = (q.Sort?.ToLower()) switch
            {
                "price" => (q.Desc ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price)),
                "quantity" => (q.Desc ? query.OrderByDescending(p => p.Quantity) : query.OrderBy(p => p.Quantity)),
                _ => (q.Desc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name))
            };
            var total = query.Count();
            var items = query.Skip((q.PageNumber - 1) * q.PageSize)
                             .Take(q.PageSize)
                             .AsQueryable()
                             .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                             .ToList();
            return new PagedResult<ProductDto> { PageNumber = q.PageNumber, PageSize = q.PageSize, TotalCount = total, Items = items };
        }
        public async Task<ProductDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id, p => p.Category);
            return entity == null ? null : _mapper.Map<ProductDto>(entity);
        }
        public async Task<ProductDto> CreateAsync(CreateProductRequest request)
        {
            var entity = _mapper.Map<Product>(request);
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return _mapper.Map<ProductDto>(entity);
        }
        public async Task<bool> UpdateAsync(Guid id, CreateProductRequest request)
        {
            var entity = await _repo.GetByIdAsync(id, p => p.Category);
            if (entity == null) return false;
            entity.Name = request.Name;
            entity.Sku = request.Sku;
            entity.Price = request.Price;
            entity.Quantity = request.Quantity;
            entity.CategoryId = request.CategoryId;
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            _repo.Remove(entity);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
