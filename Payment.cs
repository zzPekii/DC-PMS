namespace DCPMS;

public class Payment
{
    public int PaymentId { get; private set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public PaymentMethod Method { get; set; }
    public DateTime CreatedAt { get; private set; }

    public Payment(int paymentId, decimal amount, PaymentStatus status, PaymentMethod method)
    {
        PaymentId = paymentId;
        Amount = amount;
        Status = status;
        Method = method;
        CreatedAt = DateTime.Now;
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