using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models;
using WebApp.Aplication.Models.Users;
using WebApp.Domain.Enums;


namespace WebApp.Aplication.Services.Interface
{
    public interface IUserService
    {
        ResponseModel<UserAuthResponseDTO> Register(UserRegistrDTO registrDto);
        ResponseModel<UserAuthResponseDTO> RegisterByTelegram(UserRegistrDTO registrDto, string otpCode);
        ResponseModel<UserAuthResponseDTO> Login(UserLoginDTO loginDto);
        ResponseModel<UserDTO> GetUserById(int userId);
        ResponseModel<UserDTO> UpdateUser(int userId, UserUpdateDTO updateDto);
        ResponseModel<bool> DeleteUser(int userId);
        ResponseModel<IEnumerable<UserDTO>> GetAllUsers();
        ResponseModel<bool> ChangePassword(int userId, ChangePassword changePasswordDto);
        ResponseModel<string> ResetPassword(ResetPassword resetPasswordDto);
        ResponseModel<string> VerifyOtp(OtpVerificationModel model);
        ResponseModel<bool> ChangeUserRole(int userId, RoleType newRole);
    }
}
