using EcommerceWarehouse.Server.DTOs;

namespace EcommerceWarehouse.Server.Services.Interfaces
{
    public interface IStockService
    {
        Task<List<StockTransactionDto>> GetAllAsync();
        Task<StockTransactionDto> CreateAsync(CreateStockTransactionRequest request);
    }
}
