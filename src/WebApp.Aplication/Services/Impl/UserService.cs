using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Helpers.GenerateJWT;
using WebApp.Aplication.Helpers.PasswordHash;
using WebApp.Aplication.Models;
using WebApp.Aplication.Models.Users;
using WebApp.Aplication.Services.Interface;
using WebApp.DataAccess.Persistence;
using WebApp.Domain.Entities;

namespace WebApp.Aplication.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public readonly JwtService _jwtService;
        public readonly PasswordHash _passwordHash;
       // public readonly IEmailService _emailService;
        public readonly IOtpService _otpService;

        public UserService(AppDbContext context, JwtService jwtService, PasswordHash passwordHash, IEmailService emailService,IOtpService otpService)       
        {
            _context = context;
            _jwtService = jwtService;
            _passwordHash = passwordHash;
            //_emailService = emailService;
            _otpService = otpService;
        }

        public Guid Create(CreateUserModel createUserModel)
        {
            string salt = Guid.NewGuid().ToString();
            string HashPass = _passwordHash.Encrypt(createUserModel.Password, salt);

            var result = new User
            {
                PhoneNumber = createUserModel.PhoneNumber,
                Password = HashPass,
                Role = createUserModel.Role,
                Salt = salt,
                UserName = createUserModel.UserName
            };

            _context.Users.Add(result);
            _context.SaveChanges();

            return result.Id;
        }

        public PaginationResult<UserListResponceModel> GetAll(PaginationOption model)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(model.Search))
            {
                query = query.Where(s => s.UserName.Contains(model.Search));
            }
            //TODO: add filter for users lisgt based on user role

            List<UserListResponceModel> users = query
                .Skip(model.PageSize * (model.PageNumber - 1))
                .Take(model.PageSize)
                .Select(s => new UserListResponceModel
                {
                    Id = s.Id,
                    UserName = s.UserName
                })
                .ToList();

            int count = _context.Users.Count();

            return new PaginationResult<UserListResponceModel> 
            {
            Values = users,
            PageSize = model.PageSize,
            PageNumber = model.PageNumber,
            
            };
        }
        public UserResponseModel GetUser(Guid id)
        {
            UserResponseModel? User = _context.Users
                .Where(s => s.Id == id)
                .Select(s => new UserResponseModel
                {
                    Id = s.Id,
                    UserName = s.UserName
                })
                .FirstOrDefault();

            if(User == null)
            {
                throw new NotImplementedException();
            }
                return User;
        }

        public LoginResponceModel LoginAsync(LoginUserModel loginUserModel)
        {
            var user = _context.Users.FirstOrDefault(x => x.PhoneNumber == loginUserModel.PhoneNumber);

            if(user == null)
            {
                throw new NotImplementedException("Username or Email is incorrect");
            }
            string token = _jwtService.GenerateToken(user);

            return new LoginResponceModel
            {
                Phonenumber = user.PhoneNumber,
                UserName = user.UserName,
                Token = token,
            };
        }

        public async Task<ApiResult<string>> VerifyOtpAsync(OtpVerificationModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);
            if (user is null)
            {
                 return ApiResult<string>.Failure(new[] { "User not faund." });
            }
            var otp = await _otpService.GetLatestOtpAsync(user.Id, model.Code);

            if (otp is null || otp.ExpiredAt < DateTime.Now)
            return ApiResult<string>.Failure(new[] { "The code was entered incorrectly or has expired." });

            user.IsVerified = true;
            await _context.SaveChangesAsync();

            return ApiResult<string>.Success("OTP successfully verified.");

             
        }
        public async Task<ApiResult<string>> RegisterAsync(string fullname, string phoneNumber, string password, bool isAdminSite)
        {
            var exUser = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (exUser != null)
            {
                return ApiResult<string>.Failure(new[] { "Bu email bilan foydalanuvchi royxatdan otganku jigar" });
            }

            var salt = Guid.NewGuid().ToString();
            var hash = _passwordHash.Encrypt(password, salt);

            var user = new User
            {
                UserName = fullname,
                PhoneNumber = phoneNumber,
                Password = hash,
                Salt = salt,
                CreatedAt = DateTime.UtcNow,
                IsVerified = false //yangi foydalanuvchi tasdiqlanmagan kirib keladi, shunichun false qilish kk
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            //string roleName = isAdminSite ? "Admin" : "User";
            //var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.UseerRoles == roleName);
            var otp = await _otpService.GenerateAndSaveOtpAsync(user.Id);
          //  await _emailService.SendOtpAsync(phoneNumber, otp);  //otp ni numberga otkazish kk

            return ApiResult<string>.Success("Ro'yxatdan o'tdingiz. Email orqali tasdiqlang.");
        }

        public Task<ApiResult<string>> SentResetPasswordAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<string>> ForgetPasswordAsync(string email, string newPassword, int otpCode)
        {
            throw new NotImplementedException();
        }
    }
}
