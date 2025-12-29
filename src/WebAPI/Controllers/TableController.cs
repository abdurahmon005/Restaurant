using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Aplication.Models.Tables;
using WebApp.Aplication.Services.Interface;

namespace RestaurantProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }


        [HttpPost("Create")]
        public async Task<IActionResult> CreateTable(TableCreateModel model)
        {
            try
            {
                var table = await _tableService.CreateTableAsync(model);
                return Ok(table);
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


        [HttpGet("Get All")]
        public async Task<IActionResult> GetAllTables()
        {
            var tables = await _tableService.GetAllTablesAsync();
            return Ok(tables);
        }


        [HttpGet("Get by Id")]
        public async Task<IActionResult> GetTablesById(int id) {
            
            var table = await _tableService.GetByIdAsync(id);
            if (table == null)
            {
                return BadRequest("Topilmadi");
            }
            return Ok(table);
            
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var table = await _tableService.Delete(id);

            if(table == null)
            {
                return NotFound();
            }

            return Ok(table);
        }

    }
}
