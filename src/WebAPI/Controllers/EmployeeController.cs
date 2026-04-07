using Microsoft.AspNetCore.Mvc;
using WebApp.Aplication.Models;
using WebApp.Aplication.Models.Categories;
using WebApp.Aplication.Models.Employees;
using WebApp.Aplication.Services.Impl;
using WebApp.Aplication.Services.Interface;

namespace RestaurantApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeesDTO model)
        {
            try
            {
                var employee = await _employeeService.CreateAsync(model);
                return Ok(employee);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Xatolik yuz berdi: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _employeeService.GetAllAsync();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDTO dto)
        {
            try
            {
                var result = await _employeeService.Update(id, dto);
                if (result == null)
                {
                    return NotFound(new { message = "Employee topilmadi" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Xatolik yuz berdi: " + ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeeService.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new { message = "Employee topilmadi" });
            }

            return Ok(new { message = "Employee muvaffaqiyatli o'chirildi" });
        }
    }
}
