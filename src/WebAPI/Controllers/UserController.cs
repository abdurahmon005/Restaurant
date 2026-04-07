using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Aplication.Models;
using WebApp.Aplication.Models.Users;
using WebApp.Aplication.Services.Interface;
using WebApp.Domain.Enums;

namespace RestaurantApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, IOtpService otpService, IEmailService emailService)
        {
            _userService = userService;
            _otpService = otpService;
            _emailService = emailService;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserRegistrDTO registrDto)
        {
            var result = _userService.Register(registrDto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("register-by-telegram")]
        public IActionResult RegisterByTelegram([FromQuery] string otpCode, [FromBody] UserRegistrDTO registrDTO)
        {
            var result = _userService.RegisterByTelegram(registrDTO, otpCode);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLoginDTO loginDto)
        {
            var result = _userService.Login(loginDto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtpAsync([FromQuery] string userEmail)
        {
            try
            {
                var otp = _otpService.GenerateAndSaveOtp(userEmail);
                var result = await _emailService.SendOtpAsync(userEmail, otp);
                if (!result)
                    return BadRequest(new { IsSuccess = false, Message = "OTP yuborishda xatolik" });
                return Ok(new { IsSuccess = true, Message = "OTP muvaffaqiyatli yuborildi" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] OtpVerificationModel model)
        {
            var result = _userService.VerifyOtp(model);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("change-password")]
        [Authorize]
        public IActionResult ChangePassword([FromBody] ChangePassword changePasswordDto)
        {
            var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = _userService.ChangePassword(currentUserId, changePasswordDto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPassword resetPasswordDto)
        {
            var result = _userService.ResetPassword(resetPasswordDto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetUserById()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = _userService.GetUserById(userId);
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPut("me")]
        [Authorize]
        public IActionResult UpdateUser([FromBody] UserUpdateDTO updateDto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = _userService.UpdateUser(userId, updateDto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("me")]
        [Authorize]
        public IActionResult DeleteUser()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = _userService.DeleteUser(userId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminDeleteUser(int userId)
        {
            var result = _userService.DeleteUser(userId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers()
        {
            var result = _userService.GetAllUsers();
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPut("{userId}/role")]
        [Authorize(Roles = "Admin")]
        public IActionResult ChangeUserRole(int userId, [FromQuery] RoleType newRole)
        {
            var result = _userService.ChangeUserRole(userId, newRole);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        

    }
}
