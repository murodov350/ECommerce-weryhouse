namespace EcommerceWarehouse.Server.DTOs
{
    public class SupplierDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
    public class CreateSupplierRequest
    {
        public string Name { get; set; } = default!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
