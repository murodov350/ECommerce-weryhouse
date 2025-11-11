using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceWarehouse.Server.DTOs;
using EcommerceWarehouse.Server.Entities;
using EcommerceWarehouse.Server.Repositories.Interfaces;
using EcommerceWarehouse.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWarehouse.Server.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orders;
        private readonly IGenericRepository<OrderDetail> _details;
        private readonly IGenericRepository<Product> _products;
        private readonly IMapper _mapper;
        public OrderService(IGenericRepository<Order> orders, IGenericRepository<OrderDetail> details, IGenericRepository<Product> products, IMapper mapper)
        {
            _orders = orders; _details = details; _products = products; _mapper = mapper;
        }
        public async Task<List<OrderDto>> GetAllAsync()
        {
            var list = await _orders.GetAllAsync(o => o.Details);
            return await list.AsQueryable().ProjectTo<OrderDto>(_mapper.ConfigurationProvider).ToListAsync();
        }
        public async Task<OrderDto?> GetByIdAsync(Guid id)
        {
            var entity = await _orders.GetByIdAsync(id, o => o.Details);
            return entity == null ? null : _mapper.Map<OrderDto>(entity);
        }
        public async Task<OrderDto> CreateAsync(CreateOrderRequest request)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderNumber = GenerateOrderNumber(),
                CustomerName = request.CustomerName,
                Status = "Pending",
                TotalAmount = 0m
            };
            await _orders.AddAsync(order);

            foreach (var item in request.Items)
            {
                order.Details.Add(new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
                order.TotalAmount += item.UnitPrice * item.Quantity;

                var product = await _products.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Quantity; // decrement stock on order
                    _products.Update(product);
                }
            }
            await _orders.SaveChangesAsync();
            return _mapper.Map<OrderDto>(order);
        }
        public async Task<bool> UpdateStatusAsync(Guid id, UpdateOrderStatusRequest request)
        {
            var order = await _orders.GetByIdAsync(id);
            if (order == null) return false;
            order.Status = request.Status;
            _orders.Update(order);
            await _orders.SaveChangesAsync();
            return true;
        }
        private static string GenerateOrderNumber() => $"ORD-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }
}
