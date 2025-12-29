using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Services.Interface;
using WebApp.DataAccess.Persistence;
using WebApp.Domain.Entities;

namespace WebApp.Aplication.Services.Impl
{
    public class OtpService : IOtpService
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public OtpService(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public string GenerateAndSaveOtp(string userEmail)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                throw new ArgumentException("Foydalanuvchi topilmadi!");
            }

            var random = new Random();
            var otpCode = random.Next(100000, 999999).ToString();

            var otp = new UserOtps
            {
                UserId = user.Id,
                Code = otpCode,
                CreatedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMinutes(5),
                Used = false
            };

            _context.UserOtps.Add(otp);
            _context.SaveChanges();

            return otpCode;
        }

        public UserOtps? GetLatestOtp(int userId, string code)
        {
            return _context.UserOtps
                .Where(o => o.UserId == userId && o.Code == code && !o.Used)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefault();
        }
    }
}
