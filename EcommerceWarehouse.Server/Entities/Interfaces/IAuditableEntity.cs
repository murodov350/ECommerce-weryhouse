namespace EcommerceWarehouse.Server.Entities.Interfaces
{
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
