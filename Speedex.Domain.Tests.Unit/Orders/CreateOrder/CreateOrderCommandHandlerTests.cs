using Speedex.Domain.Orders;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.Repositories.Dtos;
using Speedex.Domain.Orders.UseCases.CreateOrder;
using Speedex.Domain.Products;

namespace Speedex.Domain.Tests.Unit.Orders.CreateOrder;

public class CreateOrderCommandHandlerTests
{
    [Fact]
    public void Handle_Should_Return_Success_Result_When_Order_Is_Created_Successfully()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var command = ACreateOrderCommand.Build();

        var handler = new CreateOrderCommandHandler(orderRepository);

        // Act
        var result = handler.Handle(command);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.OrderId);
    }

    [Theory]
    [InlineData("Doe", "DOE")]
    [InlineData("test", "TEST")]
    public void Handle_Should_UpperCase_LastName_When_Order_Is_Created_Successfully(string lastName, string expectedLastName)
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        Order? order = null;
        orderRepository
            .UpsertOrder(Arg.Do<Order>(
                or =>
                {
                    order = or;
                }))
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var command = ACreateOrderCommand
            .WithRecipient(ACreateOrderRecipient.WithLastName(lastName))
            .Build();

        var handler = new CreateOrderCommandHandler(orderRepository);

        // Act
        var result = handler.Handle(command);

        // Assert
        Assert.NotNull(order);
        Assert.Equal(expectedLastName, order.Recipient.LastName);
    }

    [Fact]
    public void Handle_Should_Return_Failure_Result_When_Order_Creation_Fails()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Failed });

        var command = ACreateOrderCommand.Build();

        var handler = new CreateOrderCommandHandler(orderRepository);

        // Act
        var result = handler.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.OrderId);
    }

    [Fact]
    public void Handle_Should_Set_Correct_Order_Id_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var command = ACreateOrderCommand.Build();
        var handler = new CreateOrderCommandHandler(orderRepository);

        // Act
        var result = handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.OrderId == result.OrderId));
    }

    [Fact]
    public void Handle_Should_Set_Correct_Product_Id_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var productId = new ProductId("product-1");
        var product = ACreateOrderCommandProduct.WithProductId(productId);
        var command = ACreateOrderCommand.WithProduct(product).Build();
        var handler = new CreateOrderCommandHandler(orderRepository);

        // Act
        handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Products.Any(p => p.ProductId == productId)));
    }

    [Fact]
    public void Handle_Should_Set_Correct_Product_Quantity_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var product = ACreateOrderCommandProduct.WithQuantity(2);
        var command = ACreateOrderCommand.WithProduct(product).Build();
        var handler = new CreateOrderCommandHandler(orderRepository);

        // Act
        handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Products.Any(p => p.Quantity == 2)));
    }

    [Fact]
    public void Handle_Should_Set_Correct_Delivery_Type_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var command = ACreateOrderCommand.WithDeliveryType(DeliveryType.Express).Build();
        var handler = new CreateOrderCommandHandler(orderRepository);

        // Act
        handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.DeliveryType == DeliveryType.Express));
    }

    [Fact]
    public void Handle_Should_Set_Correct_First_Name_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var recipient = ACreateOrderRecipient.WithFirstName("John");
        var command = ACreateOrderCommand.WithRecipient(recipient).Build();
        var handler = new CreateOrderCommandHandler(orderRepository);

        // Act
        handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Recipient.FirstName == "John"));
    }

    [Fact]
    public void Handle_Should_Set_Correct_Last_Name_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var recipient = ACreateOrderRecipient.WithLastName("Doe");
        var command = ACreateOrderCommand.WithRecipient(recipient).Build();
        var handler = new CreateOrderCommandHandler(orderRepository);

        // Act
        handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Recipient.LastName == "Doe"));
    }

    [Fact]
    public void Handle_Should_Set_Correct_Email_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var recipient = ACreateOrderRecipient.WithEmail("john.doe@example.com");
        var command = ACreateOrderCommand.WithRecipient(recipient).Build();
        var handler = new CreateOrderCommandHandler(orderRepository);

        // Act
        handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Recipient.Email == "john.doe@example.com"));
    }

    [Fact]
    public void Handle_Should_Set_Correct_Phone_Number_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var recipient = ACreateOrderRecipient.WithPhone("1234567890");
        var command = ACreateOrderCommand.WithRecipient(recipient).Build();
        var handler = new CreateOrderCommandHandler(orderRepository);

        // Act
        handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Recipient.PhoneNumber == "1234567890"));
    }

    [Fact]
    public void Handle_Should_Set_Correct_Address_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var recipient = ACreateOrderRecipient.WithAddress("123 Main St");
        var command = ACreateOrderCommand.WithRecipient(recipient).Build();
        var handler = new CreateOrderCommandHandler(orderRepository);

        // Act
        handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Recipient.Address == "123 Main St"));
    }

    [Fact]
    public void Handle_Should_Set_Correct_Additional_Address_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var recipient = ACreateOrderRecipient.WithAdditionalAddress("Apt 4");
        var command = ACreateOrderCommand.WithRecipient(recipient).Build();
        var handler = new CreateOrderCommandHandler(orderRepository);

        // Act
        handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Recipient.AdditionalAddress == "Apt 4"));
    }

    [Fact]
    public void Handle_Should_Set_Correct_City_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var recipient = ACreateOrderRecipient.WithCity("New York");
        var command = ACreateOrderCommand.WithRecipient(recipient).Build();
        var handler = new CreateOrderCommandHandler(orderRepository);

        // Act
        handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Recipient.City == "New York"));
    }

    [Fact]
    public void Handle_Should_Set_Correct_Country_When_Order_Is_Created()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        orderRepository
            .UpsertOrder(Arg.Any<Order>())
            .Returns(new UpsertOrderResult { Status = UpsertOrderResult.UpsertStatus.Success });

        var recipient = ACreateOrderRecipient.WithCountry("USA");
        var command = ACreateOrderCommand.WithRecipient(recipient).Build();
        var handler = new CreateOrderCommandHandler(orderRepository);

        // Act
        handler.Handle(command);

        // Assert
        orderRepository.Received(1).UpsertOrder(Arg.Is<Order>(o => o.Recipient.Country == "USA"));
    }
}