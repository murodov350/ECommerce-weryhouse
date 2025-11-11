using System.ComponentModel.DataAnnotations;

namespace EcommerceWarehouse.Server.Data
{
    public class Permission
    {
        public Guid Id { get; set; }
        [MaxLength(100)]
        public string Module { get; set; } = default!; // e.g., "User", "Role"
        [MaxLength(50)]
        public string Action { get; set; } = default!; // e.g., "View", "Create", "Update", "Delete"
        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
