namespace DCPMS;

public class PaintStore
{
    public PaintProduct[] Products { get; set; }

    public PaintStore(PaintProduct[] products)
    {
        Products = products;
    }

    public void DisplayProducts()
    {
        Console.WriteLine("===== Paint Store =====");

        foreach (PaintProduct product in Products)
        {
            product.DisplayInfo();
            Console.WriteLine("-----------------------");
        }
    }
}