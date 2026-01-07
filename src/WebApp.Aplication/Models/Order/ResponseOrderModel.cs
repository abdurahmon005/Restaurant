using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Domain.Enums;

namespace WebApp.Aplication.Models.Order
{
    public class ResponseOrderModel
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TableId { get; set; }
        public int UserId { get; set; }
        public string? WaiterName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Notes { get; set; }
        public List<OrderDetailModel> OrderDetails { get; set; } = new();
    }

    public class OrderDetailModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
