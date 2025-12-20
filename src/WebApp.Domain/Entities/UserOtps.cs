using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Domain.Entities
{
    public class UserOtps
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ExpiredAt { get; set; }
        public bool Used { get; set; }

        public User User { get; set; } = null!;

        public ICollection<UserOtps> OtpCodes { get; set; } = new List<UserOtps>();
    }

}
