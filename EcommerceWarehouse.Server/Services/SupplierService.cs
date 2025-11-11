using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceWarehouse.Server.DTOs;
using EcommerceWarehouse.Server.Entities;
using EcommerceWarehouse.Server.Repositories.Interfaces;
using EcommerceWarehouse.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWarehouse.Server.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IGenericRepository<Supplier> _repo;
        private readonly IMapper _mapper;
        public SupplierService(IGenericRepository<Supplier> repo, IMapper mapper)
        {
            _repo = repo; _mapper = mapper;
        }
        public async Task<List<SupplierDto>> GetAllAsync()
        {
            var query = (await _repo.GetAllAsync()).AsQueryable().Where(s => !s.IsDeleted);
            return await query.ProjectTo<SupplierDto>(_mapper.ConfigurationProvider).ToListAsync();
        }
        public async Task<SupplierDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return null;
            return _mapper.Map<SupplierDto>(entity);
        }
        public async Task<SupplierDto> CreateAsync(CreateSupplierRequest request)
        {
            var entity = _mapper.Map<Supplier>(request);
            entity.CreatedAt = DateTime.UtcNow;
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return _mapper.Map<SupplierDto>(entity);
        }
        public async Task<bool> UpdateAsync(Guid id, CreateSupplierRequest request)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return false;
            entity.Name = request.Name;
            entity.Phone = request.Phone;
            entity.Email = request.Email;
            entity.UpdatedAt = DateTime.UtcNow;
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return false;
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<SupplierDto>> QueryAsync(BasicQuery q)
        {
            var list = (await _repo.GetAllAsync()).Where(s => !s.IsDeleted).ToList();
            var query = list.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q.Search))
            {
                var s = q.Search.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(s) || (x.Email ?? "").ToLower().Contains(s));
            }
            query = (q.Sort?.ToLower()) switch
            {
                "name" => (q.Desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name)),
                _ => (q.Desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name))
            };
            var total = query.Count();
            var items = query.Skip((q.PageNumber - 1) * q.PageSize)
                             .Take(q.PageSize)
                             .AsQueryable()
                             .ProjectTo<SupplierDto>(_mapper.ConfigurationProvider)
                             .ToList();
            return new PagedResult<SupplierDto> { PageNumber = q.PageNumber, PageSize = q.PageSize, TotalCount = total, Items = items };
        }
    }
}
