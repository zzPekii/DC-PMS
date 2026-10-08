namespace DCPMS;

public class Program
{
    public static void Main(string[] args)
    {
        // =========================
        // 1. Create Brands
        // =========================
        Brand dulux = new Brand("Dulux");
        Brand taubmans = new Brand("Taubmans");


        // =========================
        // 2. Create Specifications
        // =========================
        PaintSpecification spec1 = new PaintSpecification("White", 10);

        PaintSpecification spec2 = new PaintSpecification("Blue", 5);

        PaintSpecification spec3 = new PaintSpecification("Black", 4);

        PaintSpecification spec4 = new PaintSpecification("Red", 2);


        // =========================
        // 3. Create Paint Products
        // =========================
        PaintProduct paint1 = new PaintProduct(
            1,
            "Wash & Wear",
            PaintType.Matte,
            spec1,
            dulux,
            100m
        );

        PaintProduct paint2 = new PaintProduct(
            2,
            "Super Enamel",
            PaintType.Gloss,
            spec2,
            dulux,
            80m
        );

        PaintProduct paint3 = new PaintProduct(
            3,
            "Easy Coat",
            PaintType.SemiGloss,
            spec3,
            taubmans,
            150m
        );

        PaintProduct paint4 = new PaintProduct(
            4,
            "Interior Paint",
            PaintType.Matte,
            spec4,
            taubmans,
            60m
        );


        // =========================
        // 4. Test PaintStore
        // =========================
        List<PaintProduct> storeProducts = new List<PaintProduct>
        {
            paint1,
            paint2,
            paint3,
            paint4
        };

        PaintStore store = new PaintStore(storeProducts);

        Console.WriteLine("===== STORE PRODUCTS =====");
        store.DisplayProducts();


        // =========================
        // 5. Create Order
        // =========================
        List<PaintProduct> orderProducts = new List<PaintProduct>
        {
            paint1,
            paint2,
            paint3,
            paint4
        };

        Order order = new Order(orderProducts);

        Console.WriteLine();
        Console.WriteLine("===== ORDER =====");
        order.DisplayOrder();


        // =========================
        // 6. Test Most Expensive Product
        // =========================
        Console.WriteLine();
        Console.WriteLine("===== MOST EXPENSIVE PRODUCT =====");

        PaintProduct mostExpensive = order.GetMostExpensivePaintProduct();

        Console.WriteLine(
            $"{mostExpensive.Name} - ${mostExpensive.Price:F2}"
        );


        // =========================
        // 7. Test Price Range
        // =========================
        Console.WriteLine();
        Console.WriteLine("===== PRODUCTS BETWEEN $70 AND $120 =====");

        List<PaintProduct> rangeProducts = order.GetProductsByPriceRange(70m, 120m);

        foreach (PaintProduct product in rangeProducts)
        {
            Console.WriteLine(
                $"{product.Name} - ${product.Price:F2}"
            );
        }


        // =========================
        // 8. Test Total Price By Paint Type
        // =========================
        Console.WriteLine();
        Console.WriteLine("===== TOTAL PRICE BY PAINT TYPE =====");

        Dictionary<PaintType, decimal> totals = order.GetTotalPriceByPaintType();

        foreach (KeyValuePair<PaintType, decimal> item in totals)
        {
            Console.WriteLine(
                $"{item.Key}: ${item.Value:F2}"
            );
        }


        // =========================
        // 9. Test RemoveProduct
        // =========================
        Console.WriteLine();
        Console.WriteLine("===== REMOVE PRODUCT ID 2 =====");

        order.RemoveProduct(2);

        order.DisplayOrder();


        // =========================
        // 10. Create User
        // =========================
        User user = new User(
            1,
            "Zhang"
        );

        user.AddOrder(order);


        // =========================
        // 11. Create Payments
        // =========================
        Payment payment1 = new Payment(
            1,
            50m,
            PaymentStatus.Completed,
            PaymentMethod.CreditCard
        );

        // Make timestamps slightly different
        Thread.Sleep(1000);

        Payment payment2 = new Payment(
            2,
            200m,
            PaymentStatus.Completed,
            PaymentMethod.Alipay
        );


        Payment payment3 = new Payment(
            3,
            120m,
            PaymentStatus.Pending,
            PaymentMethod.BankTransfer
        );


        // =========================
        // 12. Add Payments to User
        // =========================
        user.AddPayment(payment1);
        user.AddPayment(payment2);
        user.AddPayment(payment3);


        // =========================
        // 13. Latest Payment
        // =========================
        Console.WriteLine();
        Console.WriteLine("===== LATEST PAYMENT =====");

        Payment latestPayment = user.GetLatestPayment();

        latestPayment.DisplayPayment();


        // =========================
        // 14. Most Expensive Payment
        // =========================
        Console.WriteLine();
        Console.WriteLine("===== MOST EXPENSIVE PAYMENT =====");

        Payment mostExpensivePayment = user.GetMostExpensivePayment();

        mostExpensivePayment.DisplayPayment();


        // =========================
        // 15. Lowest Payment
        // =========================
        Console.WriteLine();
        Console.WriteLine("===== LOWEST PAYMENT =====");

        Payment lowestPayment = user.GetLowestPayment();

        lowestPayment.DisplayPayment();


        // =========================
        // 16. Payments Greater Than 100
        // =========================
        Console.WriteLine();
        Console.WriteLine("===== PAYMENTS GREATER THAN $100 =====");

        List<Payment> largePayments = user.GetPaymentsGreaterThan(100m);

        foreach (Payment payment in largePayments)
        {
            payment.DisplayPayment();
            Console.WriteLine("--------------------");
        }
    }
}