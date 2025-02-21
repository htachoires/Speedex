using Speedex.Api.Features.Orders.Requests;
using Speedex.Domain.Orders;
using Speedex.Domain.Orders.UseCases.GetOrders;
using Speedex.Domain.Products;

namespace Speedex.Api.Features.Orders.Mappers;

public static class GetOrdersQueryParamsMapper
{
    public static GetOrdersQuery ToQuery(this GetOrdersQueryParams queryParams)
    {
        return new GetOrdersQuery
        {
            OrderId = queryParams.OrderId is not null ? new OrderId(queryParams.OrderId) : null,
            ProductId = queryParams.ProductId is not null ? new ProductId(queryParams.ProductId) : null,
            PageIndex = queryParams.PageIndex,
            PageSize = queryParams.PageSize,
        };
    }
}