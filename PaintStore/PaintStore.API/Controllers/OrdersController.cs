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
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<List<OrderResponseDto>> GetOrders()
    {
        _logger.LogInformation("Getting all orders");

        List<OrderResponseDto> orders = _orderService.GetOrders().Select(o => ToResponseDto(o)).ToList();

        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public ActionResult<OrderResponseDto> GetOrderById(int id)
    {
        _logger.LogInformation("Getting order {OrderId}", id);

        Order? order = _orderService.GetOrderById(id);

        if (order == null)
        {
            _logger.LogWarning("Order {OrderId} not found", id);
            return NotFound("Cannot find the order");
        }

        return Ok(ToResponseDto(order));
    }

    [HttpGet("user/{userId:int}")]
    public ActionResult<List<OrderResponseDto>> GetOrdersByUserId(int userId)
    {
        List<OrderResponseDto> orders =
            _orderService.GetOrdersByUserId(userId).Select(o => ToResponseDto(o)).ToList();

        _logger.LogInformation("Found {Count} orders for user {UserId}", orders.Count, userId);

        return Ok(orders);
    }

    [HttpPost]
    public ActionResult<OrderResponseDto> CreateOrder(CreateOrderDto dto)
    {
        string? error = _orderService.ValidateOrder(dto.UserId, dto.Items);

        if (error != null)
        {
            _logger.LogWarning("Order creation failed: {Reason}", error);

            return BadRequest(error);
        }

        Order order = _orderService.AddOrder(dto.UserId, dto.Items);

        _logger.LogInformation("Order {OrderId} created for user {UserId}", order.Id, order.UserId);

        return StatusCode(201, ToResponseDto(order));
    }

    [HttpPut("{id:int}")]
    public ActionResult<OrderResponseDto> UpdateOrder(
        int id, UpdateOrderDto dto)
    {
        Order? existingOrder = _orderService.GetOrderById(id);

        if (existingOrder == null)
        {
            _logger.LogWarning("Order {OrderId} not found for update", id);

            return NotFound("Cannot find the order");
        }

        string? error = _orderService.ValidateOrder(dto.UserId, dto.Items);

        if (error != null)
        {
            _logger.LogWarning("Order {OrderId} update failed: {Reason}", id, error);

            return BadRequest(error);
        }

        bool updated = _orderService.UpdateOrder(
            id, dto.UserId, dto.Items);

        if (!updated)
        {
            _logger.LogWarning("Order {OrderId} not found for update", id);

            return NotFound("Cannot find the order");
        }

        Order? order = _orderService.GetOrderById(id);

        _logger.LogInformation("Order {OrderId} updated successfully", id);

        return Ok(ToResponseDto(order!));
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteOrder(int id)
    {
        bool deleted = _orderService.DeleteOrder(id);

        if (!deleted)
        {
            _logger.LogWarning("Order {OrderId} not found for deletion", id);

            return NotFound("Cannot find the order");
        }

        _logger.LogInformation("Order {OrderId} deleted successfully", id);

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
