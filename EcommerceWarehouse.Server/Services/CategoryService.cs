using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceWarehouse.Server.DTOs;
using EcommerceWarehouse.Server.Entities;
using EcommerceWarehouse.Server.Repositories.Interfaces;
using EcommerceWarehouse.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWarehouse.Server.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _repo;
        private readonly IMapper _mapper;
        public CategoryService(IGenericRepository<Category> repo, IMapper mapper)
        {
            _repo = repo; _mapper = mapper;
        }
        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var query = (await _repo.GetAllAsync()).AsQueryable();
            return await query.ProjectTo<CategoryDto>(_mapper.ConfigurationProvider).ToListAsync();
        }
        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<CategoryDto>(entity);
        }
        public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request)
        {
            var entity = _mapper.Map<Category>(request);
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return _mapper.Map<CategoryDto>(entity);
        }
        public async Task<bool> UpdateAsync(Guid id, CreateCategoryRequest request)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            entity.Name = request.Name;
            entity.Description = request.Description;
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
