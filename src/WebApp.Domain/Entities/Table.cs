using System;
using System.Collections.Generic;
using WebApp.Domain.Enums;

namespace WebApp.Domain.Entities
{
    public class Table
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; } = 4;
        public TableStatus Status { get; set; } = TableStatus.Available;
        public string Section { get; set; } = "Asosiy zal";

        public List<Order> Order { get; set; } = new();
        public List<Reservation> Reservations { get; set; } = new();
    }
}
