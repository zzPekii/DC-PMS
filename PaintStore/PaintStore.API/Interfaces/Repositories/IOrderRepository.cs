using PaintStore.Model;

namespace PaintStore.API.Interfaces.Repositories;

public interface IOrderRepository
{
    List<Order> GetOrders();
    Order? GetOrderById(int id);
    List<Order> GetOrdersByUserId(int userId);

    Order AddOrder(Order order);
    bool UpdateOrder(Order order);
    bool DeleteOrder(int id);
}
