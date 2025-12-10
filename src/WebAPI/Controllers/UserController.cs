using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Aplication.Models;
using WebApp.Aplication.Models.Users;
using WebApp.Aplication.Services.Interface;

namespace RestaurantProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public IActionResult PostUser(CreateUserModel usercreate)
        {
            var id = _userService.Create(usercreate);

            return Ok(id);
        }

        [Authorize(Roles = "Waiter")]
        [HttpPost("get-all")]
        public IActionResult GetAll([FromBody] PaginationOption paginationoption)
        {
            var result = _userService.GetAll(paginationoption);

            return Ok(result);
        }
        [HttpGet("{id:Guid}")]
        public IActionResult GetById([FromBody] Guid id)
        {
            var result = _userService.GetUser(id);

            return Ok(result);
        }
        [HttpPost("Login")]
        public IActionResult LoginAsync(LoginUserModel loginuser)
        {
            var result = _userService.LoginAsync(loginuser);

            return Ok(result);
        }
        [HttpPost("registr")]
        public async Task<ApiResult<string>> RegisterAsync([FromBody] RegisterUserModel model)
        {
            var result = await _userService.RegisterAsync(model.Fullname, model.PhoneNumber, model.Password, model.isAdminSite);
            return result;
        }
    }

}
