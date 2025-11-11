using EcommerceWarehouse.Server.DTOs;

namespace EcommerceWarehouse.Server.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(Guid id);
        Task<CategoryDto> CreateAsync(CreateCategoryRequest request);
        Task<bool> UpdateAsync(Guid id, CreateCategoryRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
