using PaintStore.Model;

namespace PaintStore.API.Interfaces.Repositories;

public interface IPaintProductRepository
{
    List<PaintProduct> GetPaintProducts();
    PaintProduct? GetPaintProductById(int id);
    List<PaintProduct> SearchPaintProducts(
        string? name,
        string? brand,
        decimal? minPrice,
        decimal? maxPrice);

    PaintProduct AddPaintProduct(PaintProduct product);
    bool UpdatePaintProduct(PaintProduct product);
    bool DeletePaintProduct(int id);
}
