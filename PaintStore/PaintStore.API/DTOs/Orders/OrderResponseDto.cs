using PaintStore.Model;

namespace PaintStore.API.DTOs.Orders;

public class OrderResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<OrderItem> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
}
