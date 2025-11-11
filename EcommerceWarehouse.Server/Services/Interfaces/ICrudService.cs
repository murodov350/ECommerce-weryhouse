namespace EcommerceWarehouse.Server.Services.Interfaces
{
    public interface ICrudService<TDto, TCreate>
    {
        Task<List<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(Guid id);
        Task<TDto> CreateAsync(TCreate request);
        Task<bool> UpdateAsync(Guid id, TCreate request);
        Task<bool> DeleteAsync(Guid id);
    }
}
