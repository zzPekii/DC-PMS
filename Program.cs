using DCPMS;
namespace DCPMS;

public class Program
{
    public static void Main(string[] args)
    {
        Brand dulux = new Brand("Dulux");
        Brand taubmans = new Brand("Taubmans");

        PaintSpecification spec1 = new PaintSpecification("White", 10);
        PaintSpecification spec2 = new PaintSpecification("Blue", 5);
        PaintSpecification spec3 = new PaintSpecification("Black", 4);

        PaintProduct paint1 = new PaintProduct("Wash & Wear", PaintType.Matte, spec1, dulux, 100m);
        PaintProduct paint2 = new PaintProduct("Super Enamel", PaintType.Gloss, spec2, dulux, 80m);
        PaintProduct paint3 = new PaintProduct("Easy Coat", PaintType.SemiGloss, spec3, taubmans, 70m);

        List<PaintProduct> storeProducts = new List<PaintProduct> {paint1, paint2, paint3};
        PaintStore store = new PaintStore(storeProducts);

        store.DisplayProducts();
        Console.WriteLine();

        List<PaintProduct> orderProducts = new List<PaintProduct> {paint1, paint3};

        Order order = new Order(orderProducts);
        order.DisplayOrder();
    }
}