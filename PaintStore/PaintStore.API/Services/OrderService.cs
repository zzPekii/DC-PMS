using PaintStore.API.Interfaces.Repositories;
using PaintStore.API.Interfaces.Services;
using PaintStore.Model;

namespace PaintStore.API.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPaintProductRepository _paintProductRepository;

    public OrderService(
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IPaintProductRepository paintProductRepository)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _paintProductRepository = paintProductRepository;
    }

    public List<Order> GetOrders()
    {
        return _orderRepository.GetOrders();
    }

    public Order? GetOrderById(int id)
    {
        return _orderRepository.GetOrderById(id);
    }

    public List<Order> GetOrdersByUserId(int userId)
    {
        return _orderRepository.GetOrdersByUserId(userId);
    }

    public string? ValidateOrder(int userId, List<OrderItem> items)
    {
        if (_userRepository.GetUserById(userId) == null)
        {
            return "User does not exist";
        }

        if (items == null || items.Count == 0)
        {
            return "Order must contain at least one product";
        }

        foreach (OrderItem item in items)
        {
            PaintProduct? product =
                _paintProductRepository.GetPaintProductById(item.ProductId);

            if (product == null)
            {
                return $"Product {item.ProductId} does not exist";
            }

            if (item.Quantity <= 0)
            {
                return "Product quantity must be greater than 0";
            }

            if (item.Quantity > product.Stock)
            {
                return $"Not enough stock for product {item.ProductId}";
            }
        }

        return null;
    }

    public Order AddOrder(int userId, List<OrderItem> items)
    {
        List<PaintProduct> products = new();

        foreach (OrderItem item in items)
        {
            PaintProduct product =
                _paintProductRepository.GetPaintProductById(item.ProductId)!;

            item.UnitPrice = product.GetFinalPrice();
            products.Add(product);
        }

        Order order = new Order(userId, products)
        {
            Items = items,
            Status = "Pending"
        };

        return _orderRepository.AddOrder(order);
    }

    public bool UpdateOrder(int id, int userId, List<OrderItem> items)
    {
        Order? existingOrder = _orderRepository.GetOrderById(id);

        if (existingOrder == null)
        {
            return false;
        }

        List<PaintProduct> products = new();

        foreach (OrderItem item in items)
        {
            PaintProduct product =
                _paintProductRepository.GetPaintProductById(item.ProductId)!;

            item.UnitPrice = product.GetFinalPrice();
            products.Add(product);
        }

        Order updatedOrder = new Order(userId, products)
        {
            Id = id,
            Items = items,
            Status = existingOrder.Status,
            CreatedDate = existingOrder.CreatedDate
        };

        return _orderRepository.UpdateOrder(updatedOrder);
    }

    public bool DeleteOrder(int id)
    {
        return _orderRepository.DeleteOrder(id);
    }
}
