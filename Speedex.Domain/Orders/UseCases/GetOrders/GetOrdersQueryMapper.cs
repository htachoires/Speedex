using Speedex.Domain.Orders.Repositories.Dtos;

namespace Speedex.Domain.Orders.UseCases.GetOrders;

public static class GetOrdersQueryMapper
{
    public static GetOrdersDto ToGetOrdersDto(this GetOrdersQuery query)
    {
        return new GetOrdersDto
        {
            OrderId = query.OrderId,
            ProductId = query.ProductId,
            PageIndex = query.PageIndex,
            PageSize = query.PageSize
        };
    }
}