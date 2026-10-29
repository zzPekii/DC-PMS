using PaintStore.Model;

namespace PaintStore.API.Interfaces.Services;

public interface IPaintProductService
{
    List<PaintProduct> GetPaintProducts();
    PaintProduct? GetPaintProductById(int id);
    List<PaintProduct> SearchPaintProducts(
        string? name,
        string? brand,
        decimal? minPrice,
        decimal? maxPrice);

    string? ValidatePaintProduct(PaintProduct product);
    PaintProduct AddPaintProduct(PaintProduct product);
    bool UpdatePaintProduct(PaintProduct product);
    bool DeletePaintProduct(int id);
}
