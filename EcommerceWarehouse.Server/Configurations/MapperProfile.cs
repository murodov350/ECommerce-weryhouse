using AutoMapper;
using EcommerceWarehouse.Server.DTOs;
using EcommerceWarehouse.Server.Entities;

namespace EcommerceWarehouse.Server.Configurations
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryRequest, Category>();

            CreateMap<Product, ProductDto>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name));
            CreateMap<CreateProductRequest, Product>();

            CreateMap<Supplier, SupplierDto>().ReverseMap();
            CreateMap<CreateSupplierRequest, Supplier>();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name));

            CreateMap<StockTransaction, StockTransactionDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name));
        }
    }
}
