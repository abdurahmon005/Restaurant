using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Domain.Enums;

namespace WebApp.Aplication.Models.Order
{
    public class UpdateOrderModel
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public StatusType Status { get; set; }
        public int TableId { get; set; }
        public int CustomerId { get; set; }
        public int WaiterId { get; set; }
    }
}
