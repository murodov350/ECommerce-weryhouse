namespace EcommerceWarehouse.Server.Data
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public Microsoft.AspNetCore.Identity.IdentityRole<Guid> Role { get; set; } = default!;

        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; } = default!;
    }
}
