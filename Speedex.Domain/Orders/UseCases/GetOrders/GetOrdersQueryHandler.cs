using Speedex.Domain.Commons;
using Speedex.Domain.Orders.Repositories;

namespace Speedex.Domain.Orders.UseCases.GetOrders;

public class GetOrdersQueryHandler(IOrderRepository orderRepository)
    : IQueryHandler<GetOrdersQuery, GetOrdersQueryResult>
{
    public GetOrdersQueryResult Query(GetOrdersQuery query)
    {
        var result = orderRepository.GetOrders(query.ToGetOrdersDto());

        return new GetOrdersQueryResult
        {
            Items = result
        };
    }
}