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
        private AppDbContext _context;
        private IEmailService _emailService;
        
        public OtpService(AppDbContext context, IEmailService emailService  )
        {
            _context = context;
            _emailService = emailService;
        }

        public async  Task<string> GenerateAndSaveOtpAsync(Guid userId)
        {

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            var otpCode = new Random().Next(100000, 999999).ToString();

            var otp = new UserOtps
            {
                UserId = userId,
                Code = otpCode,
                CreatedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMinutes(5)
            };

            await _context.UserOtps.AddAsync(otp);
            await _context.SaveChangesAsync();

            //await _emailService.SendOtpAsync(user.Email, otpCode);
            return otpCode;
        }


        public async Task<UserOtps?> GetLatestOtpAsync(Guid userId, string code)
        {
            return await _context.UserOtps
            .Where(o => o.UserId == userId && o.Code == code && o.ExpiredAt > DateTime.Now)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();
        }
    }
}
