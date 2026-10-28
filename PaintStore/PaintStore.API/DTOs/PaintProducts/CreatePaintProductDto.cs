using System.ComponentModel.DataAnnotations;
using PaintStore.Model;

namespace PaintStore.API.DTOs.PaintProducts;

public class CreatePaintProductDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public PaintType Type { get; set; }

    [Required]
    public string Brand { get; set; } = string.Empty;

    public string Color { get; set; } = string.Empty;

    public int SizeInLiters { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}
