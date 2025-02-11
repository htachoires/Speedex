using Speedex.Domain.Orders;
using Speedex.Domain.Orders.UseCases.GetOrders;
using Speedex.Domain.Products;

namespace Speedex.Tests.Tools.TestDataBuilders.Domain.Orders.Queries;

public class GetOrdersQueryBuilder
{
    private OrderId _orderId = new("fooOrderId");
    private ProductId _productId = new("fooProductId");
    private int? _pageIndex = 1;
    private int? _pageSize = 100;
    private string? _email = new("fooEmail");

    public static GetOrdersQueryBuilder AGetOrdersQuery => new();

    private GetOrdersQueryBuilder()
    {
    }

    public GetOrdersQuery Build()
    {
        return new GetOrdersQuery
        {
            OrderId = _orderId,
            ProductId = _productId,
            PageIndex = _pageIndex,
            PageSize = _pageSize,
        };
    }

    public GetOrdersQueryBuilder WithEmail(string email)
    {
        this._email = email;
        return this;
    }

    public GetOrdersQueryBuilder WithOrderId(OrderId orderId)
    {
        _orderId = orderId;
        return this;
    }

    public GetOrdersQueryBuilder WithProductId(ProductId productId)
    {
        _productId = productId;
        return this;
    }

    public GetOrdersQueryBuilder WithPageIndex(int? pageIndex)
    {
        _pageIndex = pageIndex;
        return this;
    }

    public GetOrdersQueryBuilder WithPageSize(int? pageSize)
    {
        _pageSize = pageSize;
        return this;
    }

    public GetOrdersQueryBuilder WithoutPageSize()
    {
        _pageSize = null;
        return this;
    }

    public GetOrdersQueryBuilder WithoutPageIndex()
    {
        _pageIndex = null;
        return this;
    }
}