using Microsoft.AspNetCore.Mvc;
using PaintStore.API.DTOs.PaintProducts;
using PaintStore.API.Interfaces.Services;
using PaintStore.Model;

namespace PaintStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaintProductsController : ControllerBase
{
    private readonly IPaintProductService _paintProductService;
    private readonly IOrderService _orderService;

    public PaintProductsController(
        IPaintProductService paintProductService,
        IOrderService orderService)
    {
        _paintProductService = paintProductService;
        _orderService = orderService;
    }

    [HttpGet]
    public ActionResult<List<PaintProductResponseDto>> GetAllPaintProducts()
    {
        List<PaintProductResponseDto> products =
            _paintProductService.GetPaintProducts()
                .Select(p => ToResponseDto(p))
                .ToList();

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public ActionResult<PaintProductResponseDto> GetPaintProductById(int id)
    {
        PaintProduct? product = _paintProductService.GetPaintProductById(id);

        if (product == null)
        {
            return NotFound("Cannot find the product");
        }

        return Ok(ToResponseDto(product));
    }

    [HttpGet("search")]
    public ActionResult<List<PaintProductResponseDto>> SearchPaintProducts(
        string? name,
        string? brand,
        decimal? minPrice,
        decimal? maxPrice)
    {
        if (minPrice.HasValue && maxPrice.HasValue &&
            minPrice.Value > maxPrice.Value)
        {
            return BadRequest("Invalid price range");
        }

        List<PaintProductResponseDto> products =
            _paintProductService.SearchPaintProducts(
                name, brand, minPrice, maxPrice)
                .Select(p => ToResponseDto(p))
                .ToList();

        return Ok(products);
    }

    [HttpPost]
    public ActionResult<PaintProductResponseDto> CreatePaintProduct(
        CreatePaintProductDto dto)
    {
        PaintProduct product = new PaintProduct(
            0,
            dto.Name,
            dto.Type,
            new PaintSpecification(dto.Color, dto.SizeInLiters),
            new Brand(dto.Brand),
            dto.Price
        )
        {
            Stock = dto.Stock
        };

        string? error = _paintProductService.ValidatePaintProduct(product);

        if (error != null)
        {
            return BadRequest(error);
        }

        PaintProduct createdProduct =
            _paintProductService.AddPaintProduct(product);

        return StatusCode(201, ToResponseDto(createdProduct));
    }

    [HttpPut("{id:int}")]
    public ActionResult<PaintProductResponseDto> UpdatePaintProduct(
        int id, UpdatePaintProductDto dto)
    {
        PaintProduct? existingProduct =
            _paintProductService.GetPaintProductById(id);

        if (existingProduct == null)
        {
            return NotFound("Cannot find the product");
        }

        PaintProduct product = new PaintProduct(
            id,
            dto.Name,
            dto.Type,
            new PaintSpecification(dto.Color, dto.SizeInLiters),
            new Brand(dto.Brand),
            dto.Price
        )
        {
            Stock = dto.Stock
        };

        string? error = _paintProductService.ValidatePaintProduct(product);

        if (error != null)
        {
            return BadRequest(error);
        }

        bool updated = _paintProductService.UpdatePaintProduct(product);

        if (!updated)
        {
            return NotFound("Cannot find the product");
        }

        return Ok(ToResponseDto(existingProduct));
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeletePaintProduct(int id)
    {
        bool deleted = _paintProductService.DeletePaintProduct(id);

        if (!deleted)
        {
            return NotFound("Cannot find the product");
        }

        return NoContent();
    }

    [HttpGet("price")]
    public ActionResult<List<PaintProductResponseDto>> GetProductsByPriceRange(
        decimal min, decimal max)
    {
        if (min > max)
        {
            return BadRequest("Invalid price range");
        }

        List<PaintProductResponseDto> products =
            _paintProductService.SearchPaintProducts(
                null, null, min, max)
                .Select(p => ToResponseDto(p))
                .ToList();

        if (products.Count == 0)
        {
            return NotFound("No products in this price range");
        }

        return Ok(products);
    }

    [HttpGet("user/{userId:int}")]
    public ActionResult<List<PaintProductResponseDto>> GetPaintProductsByUserId(
        int userId)
    {
        List<PaintProductResponseDto> products = _orderService
            .GetOrdersByUserId(userId)
            .SelectMany(o => o.PaintProducts)
            .DistinctBy(p => p.Id)
            .Select(p => ToResponseDto(p))
            .ToList();

        if (products.Count == 0)
        {
            return NotFound("No products found for this user");
        }

        return Ok(products);
    }

    private static PaintProductResponseDto ToResponseDto(
        PaintProduct product)
    {
        return new PaintProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Type = product.Type,
            Brand = product.Brand.Name,
            Color = product.Specification.Color,
            SizeInLiters = product.Specification.SizeInLiters,
            Price = product.Price,
            Stock = product.Stock,
            CreatedDate = product.CreatedDate
        };
    }
}
