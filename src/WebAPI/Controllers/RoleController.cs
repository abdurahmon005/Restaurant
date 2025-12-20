using Microsoft.AspNetCore.Mvc;
using WebApp.Aplication.Models;
using WebApp.Aplication.Models.Categories;
using WebApp.Aplication.Models.Roles;
using WebApp.Aplication.Services.Impl;
using WebApp.Aplication.Services.Interface;

namespace RestaurantApp.API.Controllers
{
    [ApiController]
    [Route("Roles")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpPost("Create Role")]

        public async Task<IActionResult> Create([FromBody] RoleCreateModel model)
        {
            var result = await _roleService.CreateRole(model);

            return Ok(ApiResult<ResponseRoleModel>.Success(result));
        }

        [HttpGet("Get All Role")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleService.GetAllAsync();

            return Ok(result);
        }

        [HttpPut("Update Role")]
        public async Task<IActionResult> Update(string name, [FromForm] RoleUpdateModel model)
        {
            var result = await _roleService.UpdateRole(name, model);

            return Ok(result);
        }

        [HttpDelete("Delete Role")]
        public async Task<IActionResult> Delete([FromQuery] RoleDeleteModel model)
        {
            var result = await _roleService.DeleteAsync(model);

            return Ok(result);
        }
    }
}
