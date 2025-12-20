using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Domain.Enums;

namespace WebApp.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public StatusType Status { get; set; }
        public decimal TotalAmount { get; set; }    
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public Table Table { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int TableId { get; set; }

        public  List<OrderDetails> OrderDetails { get; set; }
        public  List<Payment> Payments { get; set; }

    }
}
