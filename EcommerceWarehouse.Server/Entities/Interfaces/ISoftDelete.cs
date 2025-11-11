namespace EcommerceWarehouse.Server.Entities.Interfaces
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
