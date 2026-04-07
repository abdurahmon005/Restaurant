using WebApp.Aplication.Models;
using WebApp.Aplication.Models.Waiter;
using WebApp.Domain.Enums;

namespace WebApp.Aplication.Services.Interface
{
    
    public interface IWaiterOrderService
    {
        
        Task<ApiResult<WaiterOrderResponseDTO>> CreateOrderAsync(int waiterId, WaiterCreateOrderDTO dto);

        
        Task<ApiResult<WaiterOrderResponseDTO>> GetOrderByIdAsync(int orderId);

        
        Task<ApiResult<List<WaiterOrderResponseDTO>>> GetWaiterOrdersAsync(int waiterId, WaiterOrdersFilterDTO? filter = null);

        
        Task<ApiResult<List<WaiterOrderResponseDTO>>> GetActiveOrdersAsync(int waiterId);

        
        Task<ApiResult<List<WaiterOrderResponseDTO>>> GetOrdersByTableAsync(int tableId);

        
        Task<ApiResult<WaiterOrdersSummaryDTO>> GetOrdersSummaryAsync(int waiterId);

        
        Task<ApiResult<WaiterOrderResponseDTO>> UpdateOrderStatusAsync(int orderId, int waiterId, WaiterUpdateOrderStatusDTO dto);

        
        Task<ApiResult<WaiterOrderResponseDTO>> AddItemsToOrderAsync(int orderId, int waiterId, WaiterAddItemsDTO dto);

        
        Task<ApiResult<WaiterOrderResponseDTO>> RemoveItemFromOrderAsync(int orderId, int orderItemId, int waiterId);

        /// <summary>
        /// Update item quantity in order
        /// </summary>
        Task<ApiResult<WaiterOrderResponseDTO>> UpdateItemQuantityAsync(int orderId, int orderItemId, int waiterId, WaiterUpdateItemQuantityDTO dto);

        /// <summary>
        /// Update order notes
        /// </summary>
        Task<ApiResult<WaiterOrderResponseDTO>> UpdateOrderNotesAsync(int orderId, int waiterId, WaiterUpdateOrderNotesDTO dto);

        /// <summary>
        /// Cancel order
        /// </summary>
        Task<ApiResult<bool>> CancelOrderAsync(int orderId, int waiterId, string? reason = null);

        /// <summary>
        /// Get available tables for waiter
        /// </summary>
        Task<ApiResult<List<WaiterTableInfoDTO>>> GetAvailableTablesAsync();

        /// <summary>
        /// Get all tables with their status
        /// </summary>
        Task<ApiResult<List<WaiterTableInfoDTO>>> GetAllTablesAsync();
    }
}
