using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcommerceWarehouse.Server.Entities.Interfaces;

namespace EcommerceWarehouse.Server.Entities
{
    public class Product : IAuditableEntity, ISoftDelete
    {
        public Guid Id { get; set; }
        [Required, MaxLength(200)]
        public string Name { get; set; } = default!;
        [Required, MaxLength(100)]
        public string Sku { get; set; } = default!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
