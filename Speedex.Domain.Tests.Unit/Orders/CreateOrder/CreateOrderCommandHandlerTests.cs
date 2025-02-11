using FluentValidation;
using Speedex.Domain.Orders;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.Repositories.Dtos;
using Speedex.Domain.Orders.UseCases.CreateOrder;
using Speedex.Domain.Products;
using Speedex.Domain.Products.Repositories;
using Speedex.Tests.Tools.TestDataBuilders.Domain.Products;

namespace Speedex.Domain.Tests.Unit.Orders.CreateOrder;

public class CreateOrderCommandHandlerTests
{
    private readonly IValidator<CreateOrderCommand> _commandValidator = Substitute.For<IValidator<CreateOrderCommand>>();

    public CreateOrderCommandHandlerTests()
    {
        _commandValidator
            .ValidateAsync(Arg.Any<CreateOrderCommand>())
            .Returns(new FluentValidation.Results.ValidationResult());
    }

    [Fact]
    public async Task Handle_Should_Return_Success_Result_When_Order_Is_Created_Successfully()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        productRepository
            .GetProductById(Arg.Any<ProductId>(), Arg.Any<CancellationToken>())
            .Returns(ProductBuilder.AProduct.Build());

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var command = ACreateOrderCommand.Build();

        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.OrderId);
    }

    [Fact]
    public async Task Handle_Should_Compute_TotalOrderAmountInEUR()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        var productCosting10Euro = ProductBuilder.AProduct.WithPrice(10, Currency.EUR).Build();
        var productCosting5Euro = ProductBuilder.AProduct.WithPrice(5, Currency.EUR).Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p => p == productCosting10Euro.ProductId), Arg.Any<CancellationToken>())
            .Returns(productCosting10Euro);

        productRepository
            .GetProductById(Arg.Is<ProductId>(p => p == productCosting5Euro.ProductId), Arg.Any<CancellationToken>())
            .Returns(productCosting5Euro);

        Order? order = null;
        orderRepository
            .UpsertOrder(Arg.Do<Order>(x => order = x))
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var command = ACreateOrderCommand
            .WithProducts(
                ACreateOrderCommandProduct
                    .WithProductId(productCosting10Euro.ProductId)
                    .WithQuantity(2),
                ACreateOrderCommandProduct
                    .WithProductId(productCosting5Euro.ProductId)
                    .WithQuantity(4))
            .Build();

        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(order);
        Assert.Equal(40, order.TotalAmount.Amount);
    }

    [Fact]
    public async Task Handle_Should_Return_WeightExceeded_Result_When_Order_Weight_More_Than_30_Kilograms()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        productRepository
            .GetProductById(Arg.Any<ProductId>(), Arg.Any<CancellationToken>())
            .Returns(ProductBuilder.AProduct.Build());

        var product = new Product()
        {
            ProductId = new ProductId("productId"),
            Weight = new Weight()
            {
                Unit = WeightUnit.Kg,
                Value = 31
            }
        };

        productRepository
            .GetProductById(Arg.Is<ProductId>(p => p == product.ProductId), CancellationToken.None)
            .Returns(product);

        var command = ACreateOrderCommand
            .WithProducts(ACreateOrderCommandProduct
                .WithProductId(product.ProductId)
                .WithQuantity(1))
            .Build();

        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Command weight is more than 30kg", result.Errors.FirstOrDefault().Message);
        Assert.Equal("Command_WeightExceeded_Error", result.Errors.FirstOrDefault().Code);
    }

    [Fact]
    public async Task Handle_Should_Return_Success_When_Order_Weight_Less_Than_30_Kilograms_With_Product_Containing_Grams_Weight()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        var product = new Product()
        {
            ProductId = new ProductId("productId"),
            Weight = new Weight()
            {
                Unit = WeightUnit.Gr,
                Value = 31
            },
            Price = new Price
            {
                Amount = 10,
                Currency = Currency.EUR
            }
        };

        productRepository
            .GetProductById(Arg.Is<ProductId>(p => p == product.ProductId), CancellationToken.None)
            .Returns(product);

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var command = ACreateOrderCommand
            .WithProducts(ACreateOrderCommandProduct
                .WithProductId(product.ProductId)
                .WithQuantity(1))
            .Build();

        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.True(result.Success);
    }

    [Theory]
    [InlineData("Doe", "DOE")]
    [InlineData("test", "TEST")]
    public async Task Handle_Should_UpperCase_LastName_When_Order_Is_Created_Successfully(string lastName, string expectedLastName)
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        productRepository
            .GetProductById(Arg.Any<ProductId>(), Arg.Any<CancellationToken>())
            .Returns(ProductBuilder.AProduct.Build());

        Order? order = null;
        orderRepository
            .UpsertOrder(Arg.Do<Order>(or => { order = or; }))
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var command = ACreateOrderCommand
            .WithRecipient(ACreateOrderRecipient.WithLastName(lastName))
            .Build();

        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        await handler.Handle(command);

        // Assert
        Assert.NotNull(order);
        Assert.Equal(expectedLastName, order.Recipient.LastName);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_Result_When_Order_Creation_Fails()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        productRepository
            .GetProductById(Arg.Any<ProductId>(), Arg.Any<CancellationToken>())
            .Returns(ProductBuilder.AProduct.Build());

        productRepository
            .GetProductById(Arg.Any<ProductId>(), Arg.Any<CancellationToken>())
            .Returns(ProductBuilder.AProduct.Build());

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Failed });

        var command = ACreateOrderCommand.Build();

        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.OrderId);
    }

    [Fact]
    public async Task Handle_Should_Set_Correct_Order_Id_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        productRepository
            .GetProductById(Arg.Any<ProductId>(), Arg.Any<CancellationToken>())
            .Returns(ProductBuilder.AProduct.Build());

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var command = ACreateOrderCommand.Build();
        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        var result = await handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.OrderId == result.OrderId));
    }

    [Fact]
    public async Task Handle_Should_Set_Correct_Product_Id_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        productRepository
            .GetProductById(Arg.Any<ProductId>(), Arg.Any<CancellationToken>())
            .Returns(ProductBuilder.AProduct.Build());

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var productId = new ProductId("product-1");
        var product = ACreateOrderCommandProduct.WithProductId(productId);
        var command = ACreateOrderCommand.WithProduct(product).Build();
        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        await handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Products.Any(p => p.ProductId == productId)));
    }

    [Fact]
    public async Task Handle_Should_Set_Correct_Product_Quantity_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        productRepository
            .GetProductById(Arg.Any<ProductId>(), Arg.Any<CancellationToken>())
            .Returns(ProductBuilder.AProduct.Build());

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var product = ACreateOrderCommandProduct.WithQuantity(2);
        var command = ACreateOrderCommand.WithProduct(product).Build();
        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        await handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Products.Any(p => p.Quantity == 2)));
    }

    [Fact]
    public async Task Handle_Should_Set_Correct_Delivery_Type_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        productRepository
            .GetProductById(Arg.Any<ProductId>(), Arg.Any<CancellationToken>())
            .Returns(ProductBuilder.AProduct.Build());

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var command = ACreateOrderCommand.WithDeliveryType(DeliveryType.Express).Build();
        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        await handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.DeliveryType == DeliveryType.Express));
    }

    [Fact]
    public async Task Handle_Should_Set_Correct_First_Name_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        productRepository
            .GetProductById(Arg.Any<ProductId>(), Arg.Any<CancellationToken>())
            .Returns(ProductBuilder.AProduct.Build());

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var recipient = ACreateOrderRecipient.WithFirstName("John");
        var command = ACreateOrderCommand.WithRecipient(recipient).Build();
        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        await handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Recipient.FirstName == "John"));
    }

    [Fact]
    public async Task Handle_Should_Set_Correct_Email_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        productRepository
            .GetProductById(Arg.Any<ProductId>(), Arg.Any<CancellationToken>())
            .Returns(ProductBuilder.AProduct.Build());

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var recipient = ACreateOrderRecipient.WithEmail("john.doe@example.com");
        var command = ACreateOrderCommand.WithRecipient(recipient).Build();
        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        await handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Recipient.Email == "john.doe@example.com"));
    }

    [Fact]
    public async Task Handle_Should_Set_Correct_Phone_Number_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        productRepository
            .GetProductById(Arg.Any<ProductId>(), Arg.Any<CancellationToken>())
            .Returns(ProductBuilder.AProduct.Build());

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var recipient = ACreateOrderRecipient.WithPhone("1234567890");
        var command = ACreateOrderCommand.WithRecipient(recipient).Build();
        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        await handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Recipient.PhoneNumber == "1234567890"));
    }

    [Fact]
    public async Task Handle_Should_Set_Correct_Address_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        productRepository
            .GetProductById(Arg.Any<ProductId>(), Arg.Any<CancellationToken>())
            .Returns(ProductBuilder.AProduct.Build());

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var recipient = ACreateOrderRecipient.WithAddress("123 Main St");
        var command = ACreateOrderCommand.WithRecipient(recipient).Build();
        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        await handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Recipient.Address == "123 Main St"));
    }

    [Fact]
    public async Task Handle_Should_Set_Correct_Additional_Address_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        productRepository
            .GetProductById(Arg.Any<ProductId>(), Arg.Any<CancellationToken>())
            .Returns(ProductBuilder.AProduct.Build());

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var recipient = ACreateOrderRecipient.WithAdditionalAddress("Apt 4");
        var command = ACreateOrderCommand.WithRecipient(recipient).Build();
        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        await handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Recipient.AdditionalAddress == "Apt 4"));
    }

    [Fact]
    public async Task Handle_Should_Set_Correct_City_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        productRepository
            .GetProductById(Arg.Any<ProductId>(), Arg.Any<CancellationToken>())
            .Returns(ProductBuilder.AProduct.Build());

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var recipient = ACreateOrderRecipient.WithCity("New York");
        var command = ACreateOrderCommand.WithRecipient(recipient).Build();
        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        await handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Recipient.City == "New York"));
    }

    [Fact]
    public async Task Handle_Should_Set_Correct_Country_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        productRepository
            .GetProductById(Arg.Any<ProductId>(), Arg.Any<CancellationToken>())
            .Returns(ProductBuilder.AProduct.Build());

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var recipient = ACreateOrderRecipient.WithCountry("USA");
        var command = ACreateOrderCommand.WithRecipient(recipient).Build();
        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        await handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Recipient.Country == "USA"));
    }
}