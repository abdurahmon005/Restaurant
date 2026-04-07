using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Aplication.Models.Waiter;
using WebApp.Aplication.Services.Interface;

namespace RestaurantApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Waiter,Admin")]
    public class WaiterController : ControllerBase
    {
        private readonly IWaiterOrderService _waiterOrderService;

        public WaiterController(IWaiterOrderService waiterOrderService)
        {
            _waiterOrderService = waiterOrderService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("User ID not found in token");
        }

        
        [HttpPost("orders")]
        public async Task<IActionResult> CreateOrder([FromBody] WaiterCreateOrderDTO dto)
        {
            var waiterId = GetCurrentUserId();
            var result = await _waiterOrderService.CreateOrderAsync(waiterId, dto);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetOrderById), new { orderId = result.Result.Id }, result);
        }

        
        [HttpGet("orders/{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var result = await _waiterOrderService.GetOrderByIdAsync(orderId);

            if (!result.Succeeded)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        
        [HttpGet("orders")]
        public async Task<IActionResult> GetMyOrders([FromQuery] WaiterOrdersFilterDTO? filter)
        {
            var waiterId = GetCurrentUserId();
            var result = await _waiterOrderService.GetWaiterOrdersAsync(waiterId, filter);

            return Ok(result);
        }

        
        [HttpGet("orders/active")]
        public async Task<IActionResult> GetActiveOrders()
        {
            var waiterId = GetCurrentUserId();
            var result = await _waiterOrderService.GetActiveOrdersAsync(waiterId);

            return Ok(result);
        }

        
        [HttpGet("tables/{tableId}/orders")]
        public async Task<IActionResult> GetOrdersByTable(int tableId)
        {
            var result = await _waiterOrderService.GetOrdersByTableAsync(tableId);

            return Ok(result);
        }


        [HttpGet("orders/summary")]
        public async Task<IActionResult> GetOrdersSummary()
        {
            var waiterId = GetCurrentUserId();
            var result = await _waiterOrderService.GetOrdersSummaryAsync(waiterId);

            return Ok(result);
        }

        
        [HttpPatch("orders/{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] WaiterUpdateOrderStatusDTO dto)
        {
            var waiterId = GetCurrentUserId();
            var result = await _waiterOrderService.UpdateOrderStatusAsync(orderId, waiterId, dto);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        
        [HttpPost("orders/{orderId}/items")]
        public async Task<IActionResult> AddItemsToOrder(int orderId, [FromBody] WaiterAddItemsDTO dto)
        {
            var waiterId = GetCurrentUserId();
            var result = await _waiterOrderService.AddItemsToOrderAsync(orderId, waiterId, dto);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        
        [HttpDelete("orders/{orderId}/items/{orderItemId}")]
        public async Task<IActionResult> RemoveItemFromOrder(int orderId, int orderItemId)
        {
            var waiterId = GetCurrentUserId();
            var result = await _waiterOrderService.RemoveItemFromOrderAsync(orderId, orderItemId, waiterId);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        
        [HttpPatch("orders/{orderId}/items/{orderItemId}")]
        public async Task<IActionResult> UpdateItemQuantity(int orderId, int orderItemId, [FromBody] WaiterUpdateItemQuantityDTO dto)
        {
            var waiterId = GetCurrentUserId();
            var result = await _waiterOrderService.UpdateItemQuantityAsync(orderId, orderItemId, waiterId, dto);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        
        [HttpPatch("orders/{orderId}/notes")]
        public async Task<IActionResult> UpdateOrderNotes(int orderId, [FromBody] WaiterUpdateOrderNotesDTO dto)
        {
            var waiterId = GetCurrentUserId();
            var result = await _waiterOrderService.UpdateOrderNotesAsync(orderId, waiterId, dto);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        
        [HttpPost("orders/{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderId, [FromBody] string? reason = null)
        {
            var waiterId = GetCurrentUserId();
            var result = await _waiterOrderService.CancelOrderAsync(orderId, waiterId, reason);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        
        [HttpGet("tables/available")]
        public async Task<IActionResult> GetAvailableTables()
        {
            var result = await _waiterOrderService.GetAvailableTablesAsync();

            return Ok(result);
        }

        
        [HttpGet("tables")]
        public async Task<IActionResult> GetAllTables()
        {
            var result = await _waiterOrderService.GetAllTablesAsync();

            return Ok(result);
        }
    }
}
