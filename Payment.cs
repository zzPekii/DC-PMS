namespace DCPMS;

public class Payment
{
    public int PaymentId { get; private set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public PaymentMethod Method { get; set; }
    public DateTime CreatedAt { get; private set; }

    public User User { get; set; }
    public Order Order { get; set; }

    public Payment(int paymentId, decimal amount, PaymentStatus status, PaymentMethod method, User user, Order order)
    {
        PaymentId = paymentId;
        Amount = amount;
        Status = status;
        Method = method;
        CreatedAt = DateTime.Now;
        User = user;
        Order = order;
    }

    public void DisplayPayment()
    {
        Console.WriteLine($"Payment ID: {PaymentId}");
        Console.WriteLine($"Amount: ${Amount:F2}");
        Console.WriteLine($"Status: {Status}");
        Console.WriteLine($"Method: {Method}");
        Console.WriteLine($"Created At: {CreatedAt}");
    }
}