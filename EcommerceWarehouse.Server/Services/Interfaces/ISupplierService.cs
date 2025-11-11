using EcommerceWarehouse.Server.DTOs;

namespace EcommerceWarehouse.Server.Services.Interfaces
{
    public interface ISupplierService : ICrudService<SupplierDto, CreateSupplierRequest>
    {
        Task<PagedResult<SupplierDto>> QueryAsync(BasicQuery query);
    }
}
