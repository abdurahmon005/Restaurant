using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Domain.Enums;

namespace WebApp.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public int AccountId { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual Payment Payment { get; set; } = null!;
        public virtual Account Account { get; set; } = null!;
    }
}
