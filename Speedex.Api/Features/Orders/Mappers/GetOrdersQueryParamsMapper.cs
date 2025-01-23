using Speedex.Api.Features.Orders.Requests;
using Speedex.Domain.Orders;
using Speedex.Domain.Orders.UseCases.GetOrders;

namespace Speedex.Api.Features.Orders.Mappers;

public static class GetOrdersQueryParamsMapper
{
    public static GetOrdersQuery ToQuery(this GetOrdersQueryParams queryParams)
    {
        const int defaultPageIndex = 1;
        const int defaultPageSize = 100;

        return new GetOrdersQuery
        {
            OrderId = queryParams.OrderId is not null ? new OrderId(queryParams.OrderId) : null,
            PageIndex = queryParams.PageIndex ?? defaultPageIndex,
            PageSize = queryParams.PageSize ?? defaultPageSize,
        };
    }
}