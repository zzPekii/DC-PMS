using PaintStore.Model;

namespace PaintStore.API.DTOs.PaintProducts;

public class PaintProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public PaintType Type { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int SizeInLiters { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedDate { get; set; }
}
