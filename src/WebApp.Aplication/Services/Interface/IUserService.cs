using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models;
using WebApp.Aplication.Models.Users;


namespace WebApp.Aplication.Services.Interface
{
    public interface IUserService
    {
        Guid Create(CreateUserModel createUserModel);
        LoginResponceModel LoginAsync(LoginUserModel loginUserModel);
        PaginationResult<UserListResponceModel> GetAll(PaginationOption model);
        UserResponseModel GetUser(Guid id);
        Task<ApiResult<string>> VerifyOtpAsync(OtpVerificationModel model);
        Task<ApiResult<string>> RegisterAsync(string fullname, string email, string password, bool isAdminSite);
        Task<ApiResult<string>> SentResetPasswordAsync(string email);
        Task<ApiResult<string>> ForgetPasswordAsync(string email,string newPassword, int otpCode);
    }
}
