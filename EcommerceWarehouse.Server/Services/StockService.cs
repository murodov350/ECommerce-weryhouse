using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceWarehouse.Server.DTOs;
using EcommerceWarehouse.Server.Entities;
using EcommerceWarehouse.Server.Repositories.Interfaces;
using EcommerceWarehouse.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWarehouse.Server.Services
{
    public class StockService : IStockService
    {
        private readonly IGenericRepository<StockTransaction> _transactions;
        private readonly IGenericRepository<Product> _products;
        private readonly IMapper _mapper;
        public StockService(IGenericRepository<StockTransaction> transactions, IGenericRepository<Product> products, IMapper mapper)
        {
            _transactions = transactions; _products = products; _mapper = mapper;
        }
        public async Task<List<StockTransactionDto>> GetAllAsync()
        {
            var list = await _transactions.GetAllAsync(t => t.Product);
            return list.AsQueryable().ProjectTo<StockTransactionDto>(_mapper.ConfigurationProvider).ToList();
        }
        public async Task<StockTransactionDto> CreateAsync(CreateStockTransactionRequest request)
        {
            var product = await _products.GetByIdAsync(request.ProductId);
            if (product == null) throw new InvalidOperationException("Product not found");
            if (request.Type == "Out" && product.Quantity < request.Quantity)
                throw new InvalidOperationException("Insufficient stock");

            var transaction = new StockTransaction
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Type = request.Type,
                Date = DateTime.UtcNow
            };
            await _transactions.AddAsync(transaction);
            if (request.Type == "In") product.Quantity += request.Quantity; else product.Quantity -= request.Quantity;
            _products.Update(product);
            await _transactions.SaveChangesAsync();
            return _mapper.Map<StockTransactionDto>(transaction);
        }
    }
}
