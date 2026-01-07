using Microsoft.AspNetCore.Mvc;
using WebApp.Aplication.Models;
using WebApp.Aplication.Models.Categories;
using WebApp.Aplication.Models.Order;
using WebApp.Aplication.Services.Interface;

namespace RestaurantApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderModel model)
        {
            var result = await _orderService.CreateAsync(model);

            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAllAsync();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateOrderModel model)
        {
            var result = await _orderService.UpdateAsync(id, model);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] DeleteOrderModel model)
        {
            var result = await _orderService.DeleteAsync(model);

            return Ok(result);
        }
    }
}