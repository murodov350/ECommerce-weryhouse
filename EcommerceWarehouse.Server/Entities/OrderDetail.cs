using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcommerceWarehouse.Server.Entities.Interfaces;

namespace EcommerceWarehouse.Server.Entities
{
    public class OrderDetail : IAuditableEntity, ISoftDelete
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = default!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
