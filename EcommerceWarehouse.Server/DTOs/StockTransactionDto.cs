namespace EcommerceWarehouse.Server.DTOs
{
    public class StockTransactionDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Type { get; set; } = string.Empty; // In / Out
        public DateTime Date { get; set; }
    }

    public class CreateStockTransactionRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; } = string.Empty; // In / Out
    }
}
