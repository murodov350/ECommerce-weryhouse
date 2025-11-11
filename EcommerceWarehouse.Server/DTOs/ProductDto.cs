namespace EcommerceWarehouse.Server.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Sku { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = default!;
    }
    public class CreateProductRequest
    {
        public string Name { get; set; } = default!;
        public string Sku { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid CategoryId { get; set; }
    }
}
