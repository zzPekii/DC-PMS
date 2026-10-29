using PaintStore.Model;

namespace PaintStore.API.Interfaces.Services;

public interface IOrderService
{
    List<Order> GetOrders();
    Order? GetOrderById(int id);
    List<Order> GetOrdersByUserId(int userId);

    string? ValidateOrder(int userId, List<OrderItem> items);

    Order AddOrder(int userId, List<OrderItem> items);
    bool UpdateOrder(int id, int userId, List<OrderItem> items);
    bool DeleteOrder(int id);
}
