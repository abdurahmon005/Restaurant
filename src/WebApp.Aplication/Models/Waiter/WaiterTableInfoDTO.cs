using WebApp.Domain.Enums;

namespace WebApp.Aplication.Models.Waiter
{
    /// <summary>
    /// DTO for table information relevant to waiter
    /// </summary>
    public class WaiterTableInfoDTO
    {
        public int Id { get; set; }
        public string TableNumber { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool HasActiveOrder { get; set; }
        public int? ActiveOrderId { get; set; }
    }

    /// <summary>
    /// DTO for waiter's active orders summary
    /// </summary>
    public class WaiterOrdersSummaryDTO
    {
        public int TotalActiveOrders { get; set; }
        public int ClosedOrders { get; set; }
        public int PendingOrders { get; set; }
        public int PreparingOrders { get; set; }
        public int ReadyOrders { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public List<WaiterOrderResponseDTO> Orders { get; set; } = new();
    }

    /// <summary>
    /// Filter for waiter orders
    /// </summary>
    public class WaiterOrdersFilterDTO
    {
        public OrderStatus? Status { get; set; }
        public int? TableId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
