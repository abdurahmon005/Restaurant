using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Models.Users
{
    public class OtpVerificationModel
    {
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
    }
}
