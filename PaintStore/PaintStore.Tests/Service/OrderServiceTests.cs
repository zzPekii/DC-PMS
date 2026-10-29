using Xunit;
using Moq;
using FluentAssertions;
using PaintStore.API.Interfaces.Repositories;
using PaintStore.API.Services;
using PaintStore.Model;

namespace PaintStore.Tests.Services;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPaintProductRepository> _mockProductRepository;

    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockProductRepository = new Mock<IPaintProductRepository>();

        _orderService = new OrderService(
            _mockOrderRepository.Object,
            _mockUserRepository.Object,
            _mockProductRepository.Object
        );
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
    public void GetOrderById_ExistingOrder_ReturnsOrder()
    {
        // Arrange
        Order order = new Order(1, new List<PaintProduct>())
        {
            Id = 1
        };

        _mockOrderRepository
            .Setup(r => r.GetOrderById(1))
            .Returns(order);

        // Act
        Order? result = _orderService.GetOrderById(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [Fact]
    public void GetOrderById_NonExistingOrder_ReturnsNull()
    {
        // Arrange
        _mockOrderRepository
            .Setup(r => r.GetOrderById(999))
            .Returns((Order?)null);

        // Act
        Order? result = _orderService.GetOrderById(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ValidateOrder_NonExistingUser_ReturnsError()
    {
        // Arrange
        List<OrderItem> items = new()
        {
            new OrderItem { ProductId = 1, Quantity = 2 }
        };

        _mockUserRepository
            .Setup(r => r.GetUserById(999))
            .Returns((User?)null);

        // Act
        string? result = _orderService.ValidateOrder(999, items);

        // Assert
        result.Should().Be("User does not exist");
    }

    [Fact]
    public void ValidateOrder_EmptyItems_ReturnsError()
    {
        // Arrange
        _mockUserRepository
            .Setup(r => r.GetUserById(1))
            .Returns(new User(1, "Tom"));

        List<OrderItem> items = new();

        // Act
        string? result = _orderService.ValidateOrder(1, items);

        // Assert
        result.Should().Be("Order must contain at least one product");
    }

    [Fact]
    public void ValidateOrder_NonExistingProduct_ReturnsError()
    {
        // Arrange
        _mockUserRepository
            .Setup(r => r.GetUserById(1))
            .Returns(new User(1, "Tom"));

        List<OrderItem> items = new()
        {
            new OrderItem { ProductId = 999, Quantity = 2 }
        };

        // Act
        string? result = _orderService.ValidateOrder(1, items);

        // Assert
        result.Should().Be("Product 999 does not exist");
    }

    [Fact]
    public void ValidateOrder_ZeroQuantity_ReturnsError()
    {
        // Arrange
        _mockUserRepository
            .Setup(r => r.GetUserById(1))
            .Returns(new User(1, "Tom"));

        _mockProductRepository
            .Setup(r => r.GetPaintProductById(1))
            .Returns(CreateTestProduct());

        List<OrderItem> items = new()
        {
            new OrderItem { ProductId = 1, Quantity = 0 }
        };

        // Act
        string? result = _orderService.ValidateOrder(1, items);

        // Assert
        result.Should().Be(
            "Product quantity must be greater than 0");
    }

    [Fact]
    public void ValidateOrder_InsufficientStock_ReturnsError()
    {
        // Arrange
        _mockUserRepository
            .Setup(r => r.GetUserById(1))
            .Returns(new User(1, "Tom"));

        _mockProductRepository
            .Setup(r => r.GetPaintProductById(1))
            .Returns(CreateTestProduct());

        List<OrderItem> items = new()
        {
            new OrderItem { ProductId = 1, Quantity = 200 }
        };

        // Act
        string? result = _orderService.ValidateOrder(1, items);

        // Assert
        result.Should().Be("Not enough stock for product 1");
    }

    [Fact]
    public void ValidateOrder_ValidOrder_ReturnsNull()
    {
        // Arrange
        _mockUserRepository
            .Setup(r => r.GetUserById(1))
            .Returns(new User(1, "Tom"));

        _mockProductRepository
            .Setup(r => r.GetPaintProductById(1))
            .Returns(CreateTestProduct());

        List<OrderItem> items = new()
        {
            new OrderItem { ProductId = 1, Quantity = 2 }
        };

        // Act
        string? result = _orderService.ValidateOrder(1, items);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void AddOrder_ValidOrder_CalculatesTotal()
    {
        // Arrange
        PaintProduct product = CreateTestProduct();

        List<OrderItem> items = new()
        {
            new OrderItem { ProductId = 1, Quantity = 2 }
        };

        _mockProductRepository
            .Setup(r => r.GetPaintProductById(1))
            .Returns(product);

        _mockOrderRepository
            .Setup(r => r.AddOrder(It.IsAny<Order>()))
            .Returns((Order order) => order);

        // Act
        Order result = _orderService.AddOrder(1, items);

        // Assert
        result.UserId.Should().Be(1);
        result.Items.Should().HaveCount(1);
        result.Items[0].Quantity.Should().Be(2);

        result.CalculateTotalAmount().Should().Be(
            product.GetFinalPrice() * 2);

        _mockOrderRepository.Verify(
            r => r.AddOrder(It.IsAny<Order>()),
            Times.Once);
    }

    [Fact]
    public void UpdateOrder_NonExistingOrder_ReturnsFalse()
    {
        // Arrange
        _mockOrderRepository
            .Setup(r => r.GetOrderById(999))
            .Returns((Order?)null);

        List<OrderItem> items = new()
        {
            new OrderItem { ProductId = 1, Quantity = 2 }
        };

        // Act
        bool result = _orderService.UpdateOrder(999, 1, items);

        // Assert
        result.Should().BeFalse();

        _mockOrderRepository.Verify(
            r => r.UpdateOrder(It.IsAny<Order>()),
            Times.Never);
    }

    [Fact]
    public void DeleteOrder_ExistingOrder_ReturnsTrue()
    {
        // Arrange
        _mockOrderRepository
            .Setup(r => r.DeleteOrder(1))
            .Returns(true);

        // Act
        bool result = _orderService.DeleteOrder(1);

        // Assert
        result.Should().BeTrue();

        _mockOrderRepository.Verify(
            r => r.DeleteOrder(1),
            Times.Once);
    }
}
