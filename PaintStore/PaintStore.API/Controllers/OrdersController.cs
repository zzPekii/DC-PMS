using Microsoft.AspNetCore.Mvc;
using PaintStore.Model;

namespace PaintStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    // mock data
    public static readonly List<Order> Orders = new()
    {
        new Order(1, new List<PaintProduct>
        {
            PaintProductsController.PaintProducts[0],
            PaintProductsController.PaintProducts[1]
        })
        {
            Id = 1,
            CreatedDate = DateTime.Now
        },

        new Order(2, new List<PaintProduct>
        {
            PaintProductsController.PaintProducts[2]
        })
        {
            Id = 2,
            CreatedDate = DateTime.Now.AddDays(-5)
        },

        new Order(1, new List<PaintProduct>
        {
            PaintProductsController.PaintProducts[1],
            PaintProductsController.PaintProducts[3]
        })
        {
            Id = 3,
            CreatedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1)
        }
    };

    [HttpGet]
    public ActionResult<List<Order>> GetAllOrders()
    {
        System.Console.WriteLine("HTTP [GET] 200 OK /api/Orders");
        return Ok(Orders);
    }

}