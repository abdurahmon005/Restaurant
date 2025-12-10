using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Domain.Enums;

namespace WebApp.Domain.Entities
{
    public class TempUser
    {
        public Guid Id { get; set; }
        public int TelegramId { get; set; }
        public string PhoneNumber { get; set; }
        public string OtpCode { get; set; }
        public OtpStatus IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime ExpireDate { get; set; }


    }
    
}
