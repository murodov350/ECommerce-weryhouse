using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceWarehouse.Server.DTOs;
using EcommerceWarehouse.Server.Entities;
using EcommerceWarehouse.Server.Repositories.Interfaces;
using EcommerceWarehouse.Server.Services.Interfaces;

namespace EcommerceWarehouse.Server.Services
{
    public class CategoryQueryService : ICategoryQueryService
    {
        private readonly IGenericRepository<Category> _repo;
        private readonly IMapper _mapper;
        public CategoryQueryService(IGenericRepository<Category> repo, IMapper mapper)
        {
            _repo = repo; _mapper = mapper;
        }
        public async Task<PagedResult<CategoryDto>> QueryAsync(BasicQuery q)
        {
            var list = await _repo.GetAllAsync();
            var query = list.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q.Search))
            {
                var s = q.Search.ToLower();
                query = query.Where(c => c.Name.ToLower().Contains(s));
            }
            query = (q.Sort?.ToLower()) switch
            {
                "name" => (q.Desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name)),
                _ => (q.Desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name))
            };
            var total = query.Count();
            var items = query.Skip((q.PageNumber - 1) * q.PageSize)
                             .Take(q.PageSize)
                             .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                             .ToList();
            return new PagedResult<CategoryDto> { PageNumber = q.PageNumber, PageSize = q.PageSize, TotalCount = total, Items = items };
        }
    }
}
