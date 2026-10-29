namespace PaintStore.Model;

public class User
{
    public int Id { get; private set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string Name { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public List<Order> Orders { get; set; } = new();
    public List<Payment> Payments { get; set; } = new();

    public User(int userId, string name)
    {
        Id = userId;
        Name = name;
    }

    public void AddOrder(Order order)
    {
        Orders.Add(order);
    }

    public void AddPayment(Payment payment)
    {
        Payments.Add(payment);
    }

    public Payment? GetLatestPayment()
    {
        if (Payments.Count == 0)
            return null;

        Payment latestPayment = Payments[0];

        foreach (Payment payment in Payments)
        {
            if (payment.CreatedAt > latestPayment.CreatedAt)
            {
                latestPayment = payment;
            }
        }

        return latestPayment;
    }

    public Payment? GetMostExpensivePayment()
    {
        if (Payments.Count == 0)
            return null;

        Payment mostExpensivePayment = Payments[0];

        foreach (Payment payment in Payments)
        {
            if (payment.Amount > mostExpensivePayment.Amount)
            {
                mostExpensivePayment = payment;
            }
        }

        return mostExpensivePayment;
    }

    public Payment? GetLowestPayment()
    {
        if (Payments.Count == 0)
            return null;

        Payment lowestPayment = Payments[0];

        foreach (Payment payment in Payments)
        {
            if (payment.Amount < lowestPayment.Amount)
            {
                lowestPayment = payment;
            }
        }

        return lowestPayment;
    }

    public List<Payment> GetPaymentsGreaterThan(decimal amount)
    {
        List<Payment> result = new();

        foreach (Payment payment in Payments)
        {
            if (payment.Amount > amount)
            {
                result.Add(payment);
            }
        }

        return result;
    }

    public Order? GetMostExpensiveOrder()
    {
        if (Orders.Count == 0)
            return null;

        Order mostExpensiveOrder = Orders[0];

        foreach (Order order in Orders)
        {
            if (order.GetTotalOrderPrice() > mostExpensiveOrder.GetTotalOrderPrice())
            {
                mostExpensiveOrder = order;
            }
        }

        return mostExpensiveOrder;
    }

    public Order? GetLatestOrder()
    {
        if (Orders.Count == 0)
            return null;

        Order latestOrder = Orders[0];

        foreach (Order order in Orders)
        {
            if (order.CreatedDate > latestOrder.CreatedDate)
            {
                latestOrder = order;
            }
        }

        return latestOrder;
    }

    public void SetId(int id)
    {
        Id = id;
        return;
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