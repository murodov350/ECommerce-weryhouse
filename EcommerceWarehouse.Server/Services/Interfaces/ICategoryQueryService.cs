using EcommerceWarehouse.Server.DTOs;

namespace EcommerceWarehouse.Server.Services.Interfaces
{
    public interface ICategoryQueryService
    {
        Task<PagedResult<CategoryDto>> QueryAsync(BasicQuery query);
    }
}
