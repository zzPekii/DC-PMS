using Xunit;
using Moq;
using FluentAssertions;
using PaintStore.API.Interfaces.Repositories;
using PaintStore.API.Services;
using PaintStore.Model;

namespace PaintStore.Tests.Services;

public class PaintProductServiceTests
{
    private readonly Mock<IPaintProductRepository> _mockRepository;
    private readonly PaintProductService _paintProductService;

    public PaintProductServiceTests()
    {
        _mockRepository = new Mock<IPaintProductRepository>();
        _paintProductService =
            new PaintProductService(_mockRepository.Object);
    }

    private PaintProduct CreateTestProduct()
    {
        return new PaintProduct(
            1,
            "White Paint",
            default,
            new PaintSpecification("White", 4),
            new Brand("Dulux"),
            50m
        )
        {
            Stock = 100
        };
    }

    [Fact]
    public void GetPaintProductById_ExistingProduct_ReturnsProduct()
    {
        // Arrange
        PaintProduct product = CreateTestProduct();

        _mockRepository
            .Setup(r => r.GetPaintProductById(1))
            .Returns(product);

        // Act
        PaintProduct? result =
            _paintProductService.GetPaintProductById(1);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("White Paint");
    }

    [Fact]
    public void GetPaintProductById_NonExistingProduct_ReturnsNull()
    {
        // Arrange
        _mockRepository
            .Setup(r => r.GetPaintProductById(999))
            .Returns((PaintProduct?)null);

        // Act
        PaintProduct? result =
            _paintProductService.GetPaintProductById(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ValidatePaintProduct_ValidProduct_ReturnsNull()
    {
        // Arrange
        PaintProduct product = CreateTestProduct();

        // Act
        string? result =
            _paintProductService.ValidatePaintProduct(product);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ValidatePaintProduct_EmptyName_ReturnsError()
    {
        // Arrange
        PaintProduct product = CreateTestProduct();
        product.Name = "";

        // Act
        string? result =
            _paintProductService.ValidatePaintProduct(product);

        // Assert
        result.Should().Be("Product name cannot be empty");
    }

    [Fact]
    public void ValidatePaintProduct_ZeroPrice_ReturnsError()
    {
        // Arrange
        PaintProduct product = CreateTestProduct();
        product.Price = 0;

        // Act
        string? result =
            _paintProductService.ValidatePaintProduct(product);

        // Assert
        result.Should().Be("Price must be greater than 0");
    }

    [Fact]
    public void ValidatePaintProduct_NegativePrice_ReturnsError()
    {
        // Arrange
        PaintProduct product = CreateTestProduct();
        product.Price = -10;

        // Act
        string? result =
            _paintProductService.ValidatePaintProduct(product);

        // Assert
        result.Should().Be("Price must be greater than 0");
    }

    [Fact]
    public void ValidatePaintProduct_NegativeStock_ReturnsError()
    {
        // Arrange
        PaintProduct product = CreateTestProduct();
        product.Stock = -5;

        // Act
        string? result =
            _paintProductService.ValidatePaintProduct(product);

        // Assert
        result.Should().Be("Stock cannot be negative");
    }

    [Fact]
    public void ValidatePaintProduct_EmptyBrand_ReturnsError()
    {
        // Arrange
        PaintProduct product = CreateTestProduct();
        product.Brand = new Brand("");

        // Act
        string? result =
            _paintProductService.ValidatePaintProduct(product);

        // Assert
        result.Should().Be("Brand cannot be empty");
    }

    [Fact]
    public void AddPaintProduct_ValidProduct_CallsRepository()
    {
        // Arrange
        PaintProduct product = CreateTestProduct();

        _mockRepository
            .Setup(r => r.AddPaintProduct(product))
            .Returns(product);

        // Act
        PaintProduct result =
            _paintProductService.AddPaintProduct(product);

        // Assert
        result.Should().BeSameAs(product);

        _mockRepository.Verify(
            r => r.AddPaintProduct(product), Times.Once);
    }

    [Fact]
    public void DeletePaintProduct_ExistingProduct_ReturnsTrue()
    {
        // Arrange
        _mockRepository
            .Setup(r => r.DeletePaintProduct(1))
            .Returns(true);

        // Act
        bool result = _paintProductService.DeletePaintProduct(1);

        // Assert
        result.Should().BeTrue();
    }
}
