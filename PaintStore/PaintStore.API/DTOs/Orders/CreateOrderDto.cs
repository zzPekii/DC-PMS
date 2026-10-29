using System.ComponentModel.DataAnnotations;
using PaintStore.Model;

namespace PaintStore.API.DTOs.Orders;

public class CreateOrderDto
{
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    [Required]
    [MinLength(1)]
    public List<OrderItem> Items { get; set; } = new();
}
