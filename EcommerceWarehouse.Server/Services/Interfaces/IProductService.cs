using EcommerceWarehouse.Server.DTOs;

namespace EcommerceWarehouse.Server.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<PagedResult<ProductDto>> QueryAsync(ProductQuery query);
        Task<ProductDto?> GetByIdAsync(Guid id);
        Task<ProductDto> CreateAsync(CreateProductRequest request);
        Task<bool> UpdateAsync(Guid id, CreateProductRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
