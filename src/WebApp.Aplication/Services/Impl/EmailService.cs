using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Common;
using WebApp.Aplication.Services.Interface;

namespace WebApp.Aplication.Services.Impl
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _config;
        public EmailService(IOptions<EmailConfiguration> configuration)
        {
            _config = configuration.Value;
        }
        public async Task<bool> SendOtpAsync(string toEmail, string otp)
        {
            try
            {
                using var client = new SmtpClient(_config.SmtpServer, _config.Port)
                {
                    EnableSsl = _config.EnableSsl,
                    Credentials = new NetworkCredential(_config.Username, _config.Password)
                };

                var message = new MailMessage
                {
                    From = new MailAddress(_config.DefaultFromEmail, _config.DefaultFromName),
                    Subject = "Restaurant: OTP Verification Code",
                    Body = GenerateBody(otp),
                    IsBodyHtml = true
                };

                message.To.Add(toEmail);
                await client.SendMailAsync(message);
                return true;
            }
            catch (SmtpException smtpEx)
            {
                // SMTP ga oid xatolarni qayd qiling
                Console.WriteLine($"Elektron pochta yuborishda SMTP xatosi {toEmail} ga: {smtpEx.StatusCode} - {smtpEx.Message}");
                if (smtpEx.InnerException != null)
                {
                    Console.WriteLine($"Ichki istisno: {smtpEx.InnerException.Message}");
                }
                return false;
            }
            catch (Exception ex)
            {
                // Umumiy istisnolarni qayd qiling
                Console.WriteLine($"Elektron pochta yuborishda umumiy xato {toEmail} ga: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Ichki istisno: {ex.InnerException.Message}");
                }
                return false;
            }
        }
        private string GenerateBody(string otp)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"en\">");
            sb.AppendLine("<head>");
            sb.AppendLine("  <meta charset=\"UTF-8\">");
            sb.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            sb.AppendLine("  <title>Verification Code</title>");
            sb.AppendLine("  <style>");
            sb.AppendLine("    body { margin:0; padding:0; background-color:#f9f9f9; font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; }");
            sb.AppendLine("    .container { max-width:600px; margin:20px auto; background:#ffffff; border-radius:8px; overflow:hidden; }");
            sb.AppendLine("    .header { background:#333333; color:#ffffff; text-align:center; padding:20px; }");
            sb.AppendLine("    .header img { max-width:120px; height:auto; }");
            sb.AppendLine("    .content { padding:30px; color:#555555; }");
            sb.AppendLine("    .otp { font-size:32px; font-weight:bold; color:#e67e22; margin:30px 0; text-align:center; }");
            sb.AppendLine("    .footer { background:#f1f1f1; color:#888888; text-align:center; font-size:12px; padding:15px; }");
            sb.AppendLine("    a.button { display:inline-block; background:#e67e22; color:#ffffff; text-decoration:none; padding:12px 24px; border-radius:4px; font-size:16px; }");
            sb.AppendLine("    @media screen and (max-width:600px) { .container { margin:10px; } .content { padding:20px; } }");
            sb.AppendLine("  </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("  <div class=\"container\">");
            sb.AppendLine("    <div class=\"header\">");
            sb.AppendLine("      <!-- Agar logotip bo‘lsa o‘sha yerga qo‘ying -->");
            sb.AppendLine("      <img src=\"https://your-restaurant-domain.com/logo.png\" alt=\"Restaurant Logo\" />");
            sb.AppendLine("      <h2>Welcome to [Your Restaurant Name]</h2>");
            sb.AppendLine("    </div>");
            sb.AppendLine("    <div class=\"content\">");
            sb.AppendLine("      <p>Assalomu alaykum!</p>");
            sb.AppendLine("      <p>Your one-time verification code to access your account is:</p>");
            sb.AppendLine($"      <div class=\"otp\">{otp}</div>");
            sb.AppendLine("      <p>This code will expire in <strong>5 minutes</strong>. Please do not share it with anyone.</p>");
            sb.AppendLine("      <p>If you did not request this code, please ignore this email or <a href=\"https://your-restaurant-domain.com/support\">contact support</a>.</p>");
            sb.AppendLine("      <p>Thank you for choosing us — we look forward to serving you soon!</p>");
            sb.AppendLine("      <p><a class=\"button\" href=\"https://your-restaurant-domain.com\">Visit our website</a></p>");
            sb.AppendLine("    </div>");
            sb.AppendLine("    <div class=\"footer\">");
            sb.AppendLine("      &copy; 2025 [Restaurant Food]. All rights reserved.");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }

    }
}
