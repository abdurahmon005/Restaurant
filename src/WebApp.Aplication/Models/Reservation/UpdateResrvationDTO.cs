using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Models.Reservation
{
    public class UpdateResrvationDTO
    {
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public DateTime? ReservationTime { get; set; }
        public int? GuestCount { get; set; }
        public string? Notes { get; set; }
        public bool? IsConfirmed { get; set; }
        public int? TableId { get; set; }
    }
}