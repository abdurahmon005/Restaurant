using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Models.Users
{
    public class ResetPassword
    {
        public int OtpCode { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }

    }
}
