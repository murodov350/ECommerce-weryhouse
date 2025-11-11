using System.ComponentModel.DataAnnotations;
using EcommerceWarehouse.Server.Entities.Interfaces;

namespace EcommerceWarehouse.Server.Entities
{
    public class Category : IAuditableEntity, ISoftDelete
    {
        public Guid Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; } = default!;
        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
