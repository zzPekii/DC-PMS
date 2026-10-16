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
}