using Speedex.Domain.Parcels.UseCases.CreateParcel;
using FluentValidation;
using Speedex.Domain.Orders;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.Repositories.Dtos;
using Speedex.Domain.Parcels;
using Speedex.Domain.Parcels.Repositories;
using Speedex.Domain.Products;
using Speedex.Domain.Products.Repositories;
using Speedex.Domain.Tests.Unit.Parcels;
using Speedex.Tests.Tools.TestDataBuilders.Domain.Orders;
using Speedex.Tests.Tools.TestDataBuilders.Domain.Orders.Commands;
using Speedex.Tests.Tools.TestDataBuilders.Domain.Parcels;

namespace Speedex.Domain.Tests.Unit.Parcels.CreateParcel;

public class CreateParcelCommandHandlerTests
{
    private readonly IValidator<CreateParcelCommand> _parcelValidator = Substitute.For<IValidator<CreateParcelCommand>>();

    public CreateParcelCommandHandlerTests()
    {
        _parcelValidator
            .ValidateAsync(Arg.Any<CreateParcelCommand>())
            .Returns(new FluentValidation.Results.ValidationResult());
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_Result_When_Parcel_Is_Created_With_Volume_Over_1_Cubic_Meter()
    {
        // Arrange
        var parcelRepository = Substitute.For<IParcelRepository>();
        var productRepository = Substitute.For<IProductRepository>();
        var orderRepository = Substitute.For<IOrderRepository>();
        
        var product = new Product()
        {
            ProductId = new ProductId("productId"),
            Dimensions = new Dimensions
            {
                X = 1.1,
                Y = 1.1,
                Z = 1.1,
                Unit = DimensionUnit.M
            }
        };

        productRepository.GetProductById(new ProductId("productId"), CancellationToken.None).Returns(product);
        
        var handler = new CreateParcelCommandHandler(parcelRepository, productRepository, orderRepository, _parcelValidator);
        
        var parcelCommand = new CreateParcelCommand{
            OrderId = new OrderId("orderId"),
            Products = new List<CreateParcelCommand.ParcelProductCreateParcelCommand>
            {
                new()
                {
                    ProductId = product.ProductId,
                    Quantity = 1,
                }
            }
        };
        
        // Act
        var result = await handler.Handle(parcelCommand, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("The total volume cannot be more than 1 cubic meter", result.Errors.FirstOrDefault().Message);
        Assert.Equal("Parcel_VolumeExceeded_Error", result.Errors.FirstOrDefault().Code);
    }
    
    [Fact]
    public async Task Handle_Should_Return_Failure_Result_When_Parcel_Has_Products_Not_In_Command()
    {
        // Arrange
        var parcelRepository = Substitute.For<IParcelRepository>();
        var productRepository = Substitute.For<IProductRepository>();
        var orderRepository = Substitute.For<IOrderRepository>();

        var productInCommand = new Product()
        {
            ProductId = new ProductId("productInCommandId"),
        };
        
        var productNotInCommand = new Product()
        {
            ProductId = new ProductId("productNotInCommandId"),
        };

        var orderProductInCommand = AnOrderProduct.WithProductId(productInCommand.ProductId);
        var order = AnOrder.Id(new OrderId("orderId"))
            .WithProducts(new List<OrderProductBuilder>()
            {
                orderProductInCommand,
            }).Build();

        var orderQuery = new GetOrdersDto
        {
            OrderId = order.OrderId
        };
        
        productRepository.GetProductById(productInCommand.ProductId, CancellationToken.None).Returns(productInCommand);
        productRepository.GetProductById(productNotInCommand.ProductId, CancellationToken.None).Returns(productNotInCommand);
        orderRepository.GetOrders(orderQuery).Returns(new List<Order>
        {
            order
        });

        var parcelCommand = new CreateParcelCommand{
            OrderId = new OrderId("orderId"),
            Products = new List<CreateParcelCommand.ParcelProductCreateParcelCommand>
            {
                new()
                {
                    ProductId = productInCommand.ProductId,
                    Quantity = 1,
                },
                new()
                {
                    ProductId = productNotInCommand.ProductId,
                    Quantity = 1,
                }
            }
        };
        
        var parcelHandler = new CreateParcelCommandHandler(parcelRepository, productRepository, orderRepository, _parcelValidator);
        
        // Act
        var result = await parcelHandler.Handle(parcelCommand, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("One or more products in the parcel are not in the order", result.Errors.FirstOrDefault().Message);
        Assert.Equal("Parcel_ProductsNotInOrder_Error", result.Errors.FirstOrDefault().Code);
    }
    
    [Fact]
    public async Task Handle_Should_Return_Failure_Result_When_Product_Quantity_Does_Not_Match()
    {
        // Arrange
        var parcelRepository = Substitute.For<IParcelRepository>();
        var productRepository = Substitute.For<IProductRepository>();
        var orderRepository = Substitute.For<IOrderRepository>();

        var product = new Product()
        {
            ProductId = new ProductId("productInCommandId")
        };

        var orderProduct = AnOrderProduct.WithProductId(product.ProductId).WithQuantity(2);
        var order = AnOrder.Id(new OrderId("orderId"))
            .WithProducts(new List<OrderProductBuilder>()
            {
                orderProduct,
            }).Build();
        
        productRepository.GetProductById(product.ProductId, CancellationToken.None).Returns(product);
        
        var orderQuery = new GetOrdersDto
        {
            OrderId = order.OrderId
        };
        
        orderRepository.GetOrders(orderQuery).Returns(new List<Order>
        {
            order
        });
        
        
        var parcelCommand = new CreateParcelCommand{
            OrderId = new OrderId("orderId"),
            Products = new List<CreateParcelCommand.ParcelProductCreateParcelCommand>
            {
                new()
                {
                    ProductId = product.ProductId,
                    Quantity = 1,
                },
            }
        };
        
        var parcelHandler = new CreateParcelCommandHandler(parcelRepository, productRepository, orderRepository, _parcelValidator);
        
        // Act
        var result = await parcelHandler.Handle(parcelCommand, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("The quantity of at least one product does not match with the order", result.Errors.FirstOrDefault().Message);
        Assert.Equal("Parcel_ProductQuantity_Error", result.Errors.FirstOrDefault().Code);
    }
}