using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Telegram.Bot.Types;
using WebApp.Aplication.Helpers.GenerateJWT;
using WebApp.Aplication.Services.Interface;
using WebApp.Domain.Entities;
using User = WebApp.Domain.Entities.User;

namespace WebApp.Aplication.Services.Impl
{
    public class JwtTokenService : IJwtTokenService
    {
       private readonly JwtOption _jwtOption;

        public JwtTokenService(IOptions<JwtOption> jwtOption)
        {
            _jwtOption = jwtOption.Value;
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.UserRoles.First().Role.Name),
            new Claim("Phone", user.PhoneNumber)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOption.Issuer,
                audience: _jwtOption.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOption.ExpirationInSeconds),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
