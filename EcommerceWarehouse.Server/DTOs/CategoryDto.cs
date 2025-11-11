namespace EcommerceWarehouse.Server.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
    public class CreateCategoryRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
