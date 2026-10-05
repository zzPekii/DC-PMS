namespace DCPMS;

public class User
{
    public int UserId { get; private set; }
    public string Name { get; set; }

    public List<Order> Orders { get; set; }

    public User(int userId, string name)
    {
        UserId = userId;
        Name = name;
        Orders = new List<Order>();
    }

    public void AddOrder(Order order)
    {
        Orders.Add(order);
    }

    public void DisplayOrders()
    {
        Console.WriteLine($"===== Orders for {Name} =====");

        foreach (Order order in Orders)
        {
            order.DisplayOrder();
            Console.WriteLine("-------------------------");
        }
    }
}