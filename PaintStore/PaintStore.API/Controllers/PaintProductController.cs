using Microsoft.AspNetCore.Mvc;
using PaintStore.Model;

namespace PaintStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaintProductsController : ControllerBase
{
    // mock data
    public static readonly List<PaintProduct> PaintProducts = new()
    {
        new PaintProduct(1, "White Paint", default, null!, null!, 50m),
        new PaintProduct(2, "Black Paint", default, null!, null!, 80m),
        new PaintProduct(3, "Blue Paint", default, null!, null!, 120m),
        new PaintProduct(4, "Red Paint", default, null!, null!, 200m)
    };

    [HttpGet]
    public ActionResult<List<PaintProduct>> GetAllPaintProducts()
    {
        System.Console.WriteLine("HTTP [GET] 200 OK /api/PaintProducts");
        return Ok(PaintProducts);
    }

    [HttpGet("price")]
    public ActionResult<List<PaintProduct>> GetProductsByPriceRange(decimal min, decimal max)
    {
        List<PaintProduct> products = PaintProducts.Where(p => p.Price >= min && p.Price <= max).ToList();

        if (products.Count == 0)
        {
            System.Console.WriteLine("HTTP [GET] 404 NOT Found /api/PaintProducts/price");
            return NotFound("No products in this price range");
        }

        System.Console.WriteLine("HTTP [GET] 200 OK /api/PaintProducts/price");
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public ActionResult<PaintProduct> GetPaintProductsByPaintId(int id)
    {
        PaintProduct? product = PaintProducts.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            System.Console.WriteLine($"HTTP [GET] 404 NOT Found /api/PaintProducts/{id}");
            return NotFound("Can not find the product");
        }

        System.Console.WriteLine($"HTTP [GET] 200 OK /api/PaintProducts/{id}");
        return Ok(product);
    }

    [HttpGet("user/{userId:int}")]
    public ActionResult<List<PaintProduct>> GetPraintProductsByUserId(int userId)
    {
        List<PaintProduct> products = OrdersController.Orders
            .Where(o => o.UserId == userId)
            .SelectMany(o => o.PaintProducts)
            .DistinctBy(p => p.Id)
            .ToList();

        if (products.Count == 0)
        {
            System.Console.WriteLine($"HTTP [GET] 404 NOT Found /api/PaintProducts/user/{userId}");
            return NotFound("No products found for this user");
        }

        System.Console.WriteLine($"HTTP [GET] 200 OK /api/PaintProducts/user/{userId}");
        return Ok(products);
    }
}