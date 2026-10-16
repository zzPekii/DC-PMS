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

    [HttpGet("price")]
    public ActionResult<List<Order>> GetOrdersByPriceRange(decimal min, decimal max)
    {
        List<Order> orders = Orders.Where(o => o.GetTotalOrderPrice() >= min && o.GetTotalOrderPrice() <= max).ToList();

        if (orders.Count == 0)
        {
            System.Console.WriteLine("HTTP [GET] 404 NOT Found /api/Orders/price");
            return NotFound("No orders in this price range");
        }

        System.Console.WriteLine("HTTP [GET] 200 OK /api/Orders/price");
        return Ok(orders);
    }

    [HttpGet("paint/{paintId:int}")]
    public ActionResult<List<Order>> GetOrdersByPaintId(int paintId)
    {
        List<Order> orders = Orders.Where(o => o.PaintProducts.Any(p => p.Id == paintId)).ToList();

        if (orders.Count == 0)
        {
            System.Console.WriteLine($"HTTP [GET] 404 NOT Found /api/Orders/paint/{paintId}");
            return NotFound("No orders found for this product");
        }

        System.Console.WriteLine($"HTTP [GET] 200 OK /api/Orders/paint/{paintId}");
        return Ok(orders);
    }

    [HttpGet("user/{userId:int}")]
    public ActionResult<List<Order>> GetOrdersByUserId(int userId)
    {
        List<Order> orders = Orders.Where(o => o.UserId == userId).ToList();

        if (orders.Count == 0)
        {
            System.Console.WriteLine($"HTTP [GET] 404 NOT Found /api/Orders/user/{userId}");
            return NotFound("No orders found for this user");
        }

        System.Console.WriteLine($"HTTP [GET] 200 OK /api/Orders/user/{userId}");
        return Ok(orders);
    }

    [HttpGet("last-month")]
    public ActionResult<List<Order>> GetLastMonthOrders()
    {
        DateTime now = DateTime.Now;

        DateTime firstDayOfThisMonth = new DateTime(now.Year, now.Month, 1);
        DateTime firstDayOfLastMonth = firstDayOfThisMonth.AddMonths(-1);

        List<Order> orders = Orders.Where(o => o.CreatedDate >= firstDayOfLastMonth && o.CreatedDate < firstDayOfThisMonth).ToList();

        if (orders.Count == 0)
        {
            System.Console.WriteLine("HTTP [GET] 404 NOT Found /api/Orders/last-month");
            return NotFound("No orders found last month");
        }

        System.Console.WriteLine("HTTP [GET] 200 OK /api/Orders/last-month");
        return Ok(orders);
    }

    [HttpGet("date")]
    public ActionResult<List<Order>> GetOrdersByDate(DateTime date)
    {
        List<Order> orders = Orders.Where(o => o.CreatedDate.Date == date.Date).ToList();

        if (orders.Count == 0)
        {
            System.Console.WriteLine($"HTTP [GET] 404 NOT Found /api/Orders/date?date={date:yyyy-MM-dd}");
            return NotFound("No orders found on this date");
        }

        System.Console.WriteLine($"HTTP [GET] 200 OK /api/Orders/date?date={date:yyyy-MM-dd}");
        return Ok(orders);
    }
}