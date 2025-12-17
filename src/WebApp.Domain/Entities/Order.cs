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
        public int TableId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid WaiterId { get; set; }

































        public StatusType Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public virtual Table Table { get; set; }
        public virtual User Customer { get; set; }
        public virtual User Waiter { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

    }
}
