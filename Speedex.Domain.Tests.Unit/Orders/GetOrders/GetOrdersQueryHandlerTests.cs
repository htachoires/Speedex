using Speedex.Domain.Orders;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.Repositories.Dtos;
using Speedex.Domain.Orders.UseCases.GetOrders;
using Speedex.Domain.Products;
using Speedex.Tests.Tools.TestDataBuilders.Domain.Orders;

namespace Speedex.Domain.Tests.Unit.Orders.GetOrders;

public class GetOrdersQueryHandlerTests
{
    [Fact]
    public async Task Query_Should_Return_All_Orders()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        var orders = Enumerable
            .Range(1, 3)
            .Select(_ => AnOrder.Build())
            .ToList();

        orderRepository
            .GetOrders(Arg.Any<GetOrdersDto>())
            .Returns(orders);

        var query = AGetOrdersQuery.Build();

        var handler = new GetOrdersQueryHandler(orderRepository);

        // Act
        var result = await handler.Query(query);

        // Assert
        Assert.Equal(orders.Count, result.Items.Count());
    }

    [Fact]
    public void Query_Should_Pass_Correct_PageNumber_To_Repository()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        var handler = new GetOrdersQueryHandler(orderRepository);

        var query = AGetOrdersQuery.WithPageIndex(2).Build();

        // Act
        handler.Query(query);

        // Assert
        orderRepository.Received(1).GetOrders(Arg.Is<GetOrdersDto>(dto => dto.PageIndex == 2));
    }

    [Fact]
    public void Query_Should_Pass_Correct_PageSize_To_Repository()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        var query = AGetOrdersQuery.WithPageSize(10).Build();

        var handler = new GetOrdersQueryHandler(orderRepository);

        // Act
        handler.Query(query);

        // Assert
        orderRepository.Received(1).GetOrders(Arg.Is<GetOrdersDto>(dto => dto.PageSize == 10));
    }

    [Fact]
    public void Query_Should_Use_Default_PageSize_When_Not_Provided()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        var query = AGetOrdersQuery.WithoutPageSize().Build();

        var handler = new GetOrdersQueryHandler(orderRepository);

        // Act
        handler.Query(query);

        // Assert
        orderRepository.Received(1).GetOrders(Arg.Is<GetOrdersDto>(dto => dto.PageSize == 100));
    }

    [Fact]
    public void Query_Should_Use_Default_PageIndex_When_Not_Provided()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        var query = AGetOrdersQuery.WithoutPageIndex().Build();

        var handler = new GetOrdersQueryHandler(orderRepository);

        // Act
        handler.Query(query);

        // Assert
        orderRepository.Received(1).GetOrders(Arg.Is<GetOrdersDto>(dto => dto.PageIndex == 1));
    }

    [Fact]
    public void Query_Should_Pass_Correct_OrderId_To_Repository()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        var orderId = new OrderId("fooOrderId");
        var query = AGetOrdersQuery.WithOrderId(orderId).Build();

        var handler = new GetOrdersQueryHandler(orderRepository);

        // Act
        handler.Query(query);

        // Assert
        orderRepository.Received(1).GetOrders(Arg.Is<GetOrdersDto>(dto => dto.OrderId == orderId));
    }

    [Fact]
    public void Query_Should_Pass_Correct_ProductId_To_Repository()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();

        var productId = new ProductId("fooOrderId");
        var query = AGetOrdersQuery.WithProductId(productId).Build();

        var handler = new GetOrdersQueryHandler(orderRepository);

        // Act
        handler.Query(query);

        // Assert
        orderRepository.Received(1).GetOrders(Arg.Is<GetOrdersDto>(dto => dto.ProductId == productId));
    }
}