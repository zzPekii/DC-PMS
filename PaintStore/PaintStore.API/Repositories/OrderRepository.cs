using PaintStore.API.Data;
using PaintStore.API.Interfaces.Repositories;
using PaintStore.Model;

namespace PaintStore.API.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly InMemoryStore _store;

    public OrderRepository(InMemoryStore store)
    {
        _store = store;
    }

    public List<Order> GetOrders()
    {
        return _store.Orders;
    }

    public Order? GetOrderById(int id)
    {
        return _store.Orders.FirstOrDefault(o => o.Id == id);
    }

    public List<Order> GetOrdersByUserId(int userId)
    {
        return _store.Orders
            .Where(o => o.UserId == userId)
            .ToList();
    }

    public Order AddOrder(Order order)
    {
        int nextId = _store.Orders.Count == 0
            ? 1
            : _store.Orders.Max(o => o.Id) + 1;

        order.Id = nextId;
        _store.Orders.Add(order);

        return order;
    }

    public bool UpdateOrder(Order order)
    {
        Order? existingOrder = GetOrderById(order.Id);

        if (existingOrder == null)
        {
            return false;
        }

        existingOrder.UserId = order.UserId;
        existingOrder.Items = order.Items;
        existingOrder.PaintProducts = order.PaintProducts;
        existingOrder.Status = order.Status;

        return true;
    }

    public bool DeleteOrder(int id)
    {
        Order? order = GetOrderById(id);

        if (order == null)
        {
            return false;
        }

        return _store.Orders.Remove(order);
    }
}
