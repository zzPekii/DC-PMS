
using Microsoft.AspNetCore.Mvc;
using PaintStore.API.DTOs.Orders;
using PaintStore.API.Interfaces.Services;
using PaintStore.Model;

namespace PaintStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public ActionResult<List<OrderResponseDto>> GetOrders()
    {
        List<OrderResponseDto> orders = _orderService.GetOrders()
            .Select(o => ToResponseDto(o))
            .ToList();

        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public ActionResult<OrderResponseDto> GetOrderById(int id)
    {
        Order? order = _orderService.GetOrderById(id);

        if (order == null)
        {
            return NotFound("Cannot find the order");
        }

        return Ok(ToResponseDto(order));
    }

    [HttpGet("user/{userId:int}")]
    public ActionResult<List<OrderResponseDto>> GetOrdersByUserId(int userId)
    {
        List<OrderResponseDto> orders =
            _orderService.GetOrdersByUserId(userId)
                .Select(o => ToResponseDto(o))
                .ToList();

        return Ok(orders);
    }

    [HttpPost]
    public ActionResult<OrderResponseDto> CreateOrder(CreateOrderDto dto)
    {
        string? error = _orderService.ValidateOrder(dto.UserId, dto.Items);

        if (error != null)
        {
            return BadRequest(error);
        }

        Order order = _orderService.AddOrder(dto.UserId, dto.Items);

        return StatusCode(201, ToResponseDto(order));
    }

    [HttpPut("{id:int}")]
    public ActionResult<OrderResponseDto> UpdateOrder(
        int id, UpdateOrderDto dto)
    {
        Order? existingOrder = _orderService.GetOrderById(id);

        if (existingOrder == null)
        {
            return NotFound("Cannot find the order");
        }

        string? error = _orderService.ValidateOrder(dto.UserId, dto.Items);

        if (error != null)
        {
            return BadRequest(error);
        }

        bool updated = _orderService.UpdateOrder(
            id, dto.UserId, dto.Items);

        if (!updated)
        {
            return NotFound("Cannot find the order");
        }

        Order? order = _orderService.GetOrderById(id);

        return Ok(ToResponseDto(order!));
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteOrder(int id)
    {
        bool deleted = _orderService.DeleteOrder(id);

        if (!deleted)
        {
            return NotFound("Cannot find the order");
        }

        return NoContent();
    }

    private static OrderResponseDto ToResponseDto(Order order)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            UserId = order.UserId,
            CreatedDate = order.CreatedDate,
            Status = order.Status,
            Items = order.Items,
            TotalAmount = order.CalculateTotalAmount()
        };
    }
}
