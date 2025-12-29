using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Domain.Entities;

namespace WebApp.Aplication.Services.Interface
{
    public interface  IOtpService
    {
        string GenerateAndSaveOtp(string userEmail);
        public UserOtps? GetLatestOtp(int userId, string code);
    }
}
