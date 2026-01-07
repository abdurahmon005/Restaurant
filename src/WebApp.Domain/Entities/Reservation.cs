using System;

namespace WebApp.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        public DateTime ReservationTime { get; set; }
        public int GuestCount { get; set; }
        public string? Notes { get; set; }
        public bool IsConfirmed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int TableId { get; set; }
        public Table Table { get; set; } = null!;
    }
}
