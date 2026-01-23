using System.ComponentModel.DataAnnotations;

namespace WebApp.Aplication.Models.Waiter
{
    
    public class WaiterCreateOrderDTO
    {
        [Required]
        public int TableId { get; set; }

        public string? Notes { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Order must contain at least one item")]
        public List<WaiterOrderItemDTO> Items { get; set; } = new();
    }

    
    public class WaiterOrderItemDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        public string? SpecialInstructions { get; set; }
    }
}
