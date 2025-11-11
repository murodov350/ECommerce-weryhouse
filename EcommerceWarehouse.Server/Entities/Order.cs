using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcommerceWarehouse.Server.Entities.Interfaces;

namespace EcommerceWarehouse.Server.Entities
{
    public class Order : IAuditableEntity, ISoftDelete
    {
        public Guid Id { get; set; }
        [Required, MaxLength(50)]
        public string OrderNumber { get; set; } = default!;
        [Required, MaxLength(200)]
        public string CustomerName { get; set; } = default!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        [Required, MaxLength(30)]
        public string Status { get; set; } = "Pending";

        public ICollection<OrderDetail> Details { get; set; } = new List<OrderDetail>();

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
