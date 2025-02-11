using FluentValidation;
using Speedex.Domain.Orders;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.Repositories.Dtos;
using Speedex.Domain.Orders.UseCases.CreateOrder;
using Speedex.Domain.Products;
using Speedex.Domain.Products.Repositories;

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

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var product = AProduct.Build();
        
        productRepository
            .GetProductById(Arg.Is<ProductId>(p=> true), CancellationToken.None)
            .Returns(product);

        var command = ACreateOrderCommand.Build();

        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.OrderId);
    }
    
    [Fact]
    public async Task Handle_Should_Return_Order_With_Correct_Amount_of_Products()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        Order? order = null;
        orderRepository
            .UpsertOrder(Arg.Do<Order>(or => { order = or; }))
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });
        var id1 = new ProductId("product1");
        var id2 = new ProductId("product2");
        var qty1 = 1;
        var qty2 = 2;
        var orderProduct1 = ACreateOrderCommandProduct
            .WithProductId(id1)
            .WithQuantity(qty1);
        var orderProduct2 = ACreateOrderCommandProduct
            .WithProductId(id2)
            .WithQuantity(qty2);
        var command = ACreateOrderCommand
            .WithProducts(orderProduct1, orderProduct2)
            .Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p => p == id1),
                CancellationToken.None)
            .Returns(AProduct.Id(id1).Build());
        
        productRepository
            .GetProductById(Arg.Is<ProductId>(p => p == id2),
                CancellationToken.None)
            .Returns(AProduct.Id(id2).Build());
        
        
        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(order!.Products);
        Assert.True(order.Products.All(product => product.ProductId == id1 ? product.Quantity == qty1 : product.ProductId == id2 && product.Quantity == qty2));
    }

    [Fact]
    public async Task Handle_Should_Return_WeightExceeded_Result_When_Order_Weight_More_Than_30()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();


        var product = AProduct.WithWeight(31, WeightUnit.Kg).Build();
        
        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        productRepository
            .GetProductById(Arg.Is<ProductId>(p=> true), CancellationToken.None)
            .Returns(product);

        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        var command = ACreateOrderCommand
            .WithProduct(ACreateOrderCommandProduct.WithProductId(product.ProductId).WithQuantity(1))
            .Build();
        
        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Command weight is more than 30kg", result.Errors.FirstOrDefault().Message);
        Assert.Equal("Command_WeightExceeded_Error", result.Errors.FirstOrDefault().Code);
    }
    
    [Fact]
    public async Task Handle_Should_Return_True_If_Order_Contains_Given_Products()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();
        
        var product = AProduct.Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p=> true), CancellationToken.None)
            .Returns(product);

        Order? order = null;
        orderRepository
            .UpsertOrder(Arg.Do<Order>(or => { order = or; }))
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var command = ACreateOrderCommand
            .WithProduct(ACreateOrderCommandProduct.WithProductId(product.ProductId).WithQuantity(1))
            .Build();

        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.Contains(order!.Products, p => p.ProductId == product.ProductId);
    }
    
    [Fact]
    public async Task Handle_Should_Return_VolumeExceeded_Result_When_Order_Mesures_More_Than_1_m3()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        var product = AProduct.WithDimensions(2, 2, 2, DimensionUnit.M).Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p=> true), CancellationToken.None)
            .Returns(product);

        var command = ACreateOrderCommand
            .WithProduct(ACreateOrderCommandProduct.WithProductId(product.ProductId).WithQuantity(1))
            .Build();

        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Command volume is more than 1m3", result.Errors.FirstOrDefault().Message);
        Assert.Equal("Command_VolumeExceeded_Error", result.Errors.FirstOrDefault().Code);
    }

    [Theory]
    [InlineData("Doe", "DOE")]
    [InlineData("test", "TEST")]
    
    public async Task Handle_Should_UpperCase_LastName_When_Order_Is_Created_Successfully(string lastName, string expectedLastName)
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();


        var product = AProduct.Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p=> true), CancellationToken.None)
            .Returns(product);

        Order? order = null;
        orderRepository
            .UpsertOrder(Arg.Do<Order>(or => { order = or; }))
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var command = ACreateOrderCommand
            .WithRecipient(ACreateOrderRecipient.WithLastName(lastName))
            .Build();

        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(order!.Recipient);
        Assert.Equal(expectedLastName, order.Recipient.LastName);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_Result_When_Order_Creation_Fails()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();


        var product = AProduct.Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p => p == product.ProductId), CancellationToken.None)
            .Returns(product);

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


        var product = AProduct.Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p=> true), CancellationToken.None)
            .Returns(product);

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

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        
        var productId = new ProductId("product-1");
        var orderProduct = ACreateOrderCommandProduct.WithProductId(productId);
        var command = ACreateOrderCommand.WithProduct(orderProduct).Build();
        var product = AProduct.Id(productId).Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p=> product.ProductId == p), CancellationToken.None)
            .Returns(product);
        
        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        await handler.Handle(command);

        // Assert
        orderRepository.UpsertOrder(Arg.Is<Order>(o => o.Products.Any(p => p.ProductId == productId)));
    }

    [Fact]
    public async Task Handle_Should_Set_Correct_Product_Quantity_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();

        var orderProduct = ACreateOrderCommandProduct.WithQuantity(2);
        var command = ACreateOrderCommand.WithProduct(orderProduct).Build();
        var product = AProduct.Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p=> true), CancellationToken.None)
            .Returns(product);

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        
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


        var product = AProduct.Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p=> true), CancellationToken.None)
            .Returns(product);

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


        var product = AProduct.Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p=> true), CancellationToken.None)
            .Returns(product);

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


        var product = AProduct.Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p=> true), CancellationToken.None)
            .Returns(product);

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


        var product = AProduct.Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p=> true), CancellationToken.None)
            .Returns(product);

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


        var product = AProduct.Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p => true), CancellationToken.None)
            .Returns(product);
        
        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var recipient = ACreateOrderRecipient.WithAddress("123 Main St");
        var command = ACreateOrderCommand.WithRecipient(recipient).Build();
        var handler = new CreateOrderCommandHandler(orderRepository, _commandValidator, productRepository);

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Recipient.Address == "123 Main St"));
    }

    [Fact]
    public async Task Handle_Should_Set_Correct_Additional_Address_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var productRepository = Substitute.For<IProductRepository>();


        var product = AProduct.Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p=> true), CancellationToken.None)
            .Returns(product);

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


        var product = AProduct.Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p=> true), CancellationToken.None)
            .Returns(product);

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


        var product = AProduct.Build();

        productRepository
            .GetProductById(Arg.Is<ProductId>(p=> true), CancellationToken.None)
            .Returns(product);

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