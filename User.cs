namespace DCPMS;

public class User
{
    public int UserId { get; private set; }
    public string Name { get; set; }

    public List<Order> Orders { get; set; }
    public List<Payment> Payments { get; set; }

    public User(int userId, string name)
    {
        UserId = userId;
        Name = name;
        Orders = new List<Order>();
        Payments = new List<Payment>();
    }

    public void AddOrder(Order order)
    {
        Orders.Add(order);
    }

    public void AddPayment(Payment payment)
    {
        Payments.Add(payment);
    }

    public Payment GetLatestPayment()
    {
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

    public Payment GetMostExpensivePayment()
    {
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

    public Payment GetLowestPayment()
    {
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
        List<Payment> result = new List<Payment>();

        foreach (Payment payment in Payments)
        {
            if (payment.Amount > amount)
            {
                result.Add(payment);
            }
        }

        return result;
    }

    public Order GetMostExpensiveOrder()
    {
        Order mostExpensiveOrder = Orders[0];
        foreach(Order order in Orders)
        {
            if (order.GetTotalOrderPrice() > mostExpensiveOrder.GetTotalOrderPrice())
            {
                mostExpensiveOrder = order;
            } 
        }
        return mostExpensiveOrder;
    }

    public Order GetLatestOrder()
    {
        Order latestOrder = Orders[0];

        foreach (Order order in Orders)
        {
            if (order.CreatedAt > latestOrder.CreatedAt)
            {
                latestOrder = order;
            }
        }

        return latestOrder;
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