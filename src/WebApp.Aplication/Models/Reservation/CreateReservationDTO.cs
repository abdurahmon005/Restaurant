using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Models.Reservation
{
    public class CreateReservationDTO
    {
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        public DateTime ReservationTime { get; set; }
        public int GuestCount { get; set; }
        public string? Notes { get; set; }
        public int TableId { get; set; }
    }
}
