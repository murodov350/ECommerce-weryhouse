using System.ComponentModel.DataAnnotations;
using EcommerceWarehouse.Server.Entities.Interfaces;

namespace EcommerceWarehouse.Server.Entities
{
    public class StockTransaction : IAuditableEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public int Quantity { get; set; }
        [Required, MaxLength(10)]
        public string Type { get; set; } = default!; // In / Out
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
