using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Services.Interface
{
    public interface IEmailService
    {
        Task<bool> SendOtpAsync(string toEmail, string otp);
    }
}
