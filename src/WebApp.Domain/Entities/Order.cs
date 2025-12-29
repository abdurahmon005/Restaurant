using System;
using System.Collections.Generic;
using WebApp.Domain.Enums;

namespace WebApp.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public StatusType Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
        public string? WaiterName { get; set; }

        public Table Table { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int TableId { get; set; }

        public List<OrderDetails> OrderDetails { get; set; } = new();
        public List<Payment> Payments { get; set; } = new();
    }
}
