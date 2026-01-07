using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Models.Reservation
{
    public class ReservationResponceDTO
    {
        public int Id {  get; set; }
        public string CustomerName {  get; set; }
        public DateTime ReservationTime { get; set; }
        public int TableId {  get; set; }
        public string Message { get; set; }
    }
}
