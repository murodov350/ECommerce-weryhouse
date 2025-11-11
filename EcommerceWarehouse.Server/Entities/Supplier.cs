using System.ComponentModel.DataAnnotations;
using EcommerceWarehouse.Server.Entities.Interfaces;

namespace EcommerceWarehouse.Server.Entities
{
    public class Supplier : IAuditableEntity, ISoftDelete
    {
        public Guid Id { get; set; }
        [Required, MaxLength(150)]
        public string Name { get; set; } = default!;
        [MaxLength(30)]
        public string? Phone { get; set; }
        [EmailAddress, MaxLength(200)]
        public string? Email { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
