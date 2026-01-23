using System.ComponentModel.DataAnnotations;
using WebApp.Domain.Enums;

namespace WebApp.Aplication.Models.Waiter
{
    /// <summary>
    /// DTO for updating order status by waiter
    /// </summary>
    public class WaiterUpdateOrderStatusDTO
    {
        [Required]
        public OrderStatus NewStatus { get; set; }

        public string? Notes { get; set; }
    }

    /// <summary>
    /// DTO for adding items to existing order
    /// </summary>
    public class WaiterAddItemsDTO
    {
        [Required]
        [MinLength(1, ErrorMessage = "Must add at least one item")]
        public List<WaiterOrderItemDTO> Items { get; set; } = new();
    }

    /// <summary>
    /// DTO for updating order notes
    /// </summary>
    public class WaiterUpdateOrderNotesDTO
    {
        public string? Notes { get; set; }
    }

    /// <summary>
    /// DTO for updating item quantity
    /// </summary>
    public class WaiterUpdateItemQuantityDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
