using Speedex.Domain.Orders.Repositories.Dtos;

namespace Speedex.Domain.Orders.UseCases.GetOrders;

public static class GetOrdersQueryMapper
{
    public static GetOrdersDto ToGetOrdersDto(this GetOrdersQuery query)
    {
        const int defaultPageIndex = 1;
        const int defaultPageSize = 100;

        return new GetOrdersDto
        {
            OrderId = query.OrderId,
            ProductId = query.ProductId,
            CustomerEmail = query.CustomerEmail,
            PageIndex = query.PageIndex ?? defaultPageIndex,
            PageSize = query.PageSize ?? defaultPageSize,
        };
    }
}