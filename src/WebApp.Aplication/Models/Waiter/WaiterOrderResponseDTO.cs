using WebApp.Domain.Enums;

namespace WebApp.Aplication.Models.Waiter
{
    /// <summary>
    /// Response DTO for waiter order operations
    /// </summary>
    public class WaiterOrderResponseDTO
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string TableName { get; set; } = string.Empty;
        public int WaiterId { get; set; }
        public string WaiterName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<WaiterOrderItemResponseDTO> Items { get; set; } = new();
    }

    /// <summary>
    /// Response DTO for order item details
    /// </summary>
    public class WaiterOrderItemResponseDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? SpecialInstructions { get; set; }
    }
}
