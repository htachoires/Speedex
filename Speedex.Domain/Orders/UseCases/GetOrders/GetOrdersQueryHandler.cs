using Speedex.Domain.Commons;
using Speedex.Domain.Orders.Repositories;

namespace Speedex.Domain.Orders.UseCases.GetOrders;

public class GetOrdersQueryHandler(IOrderRepository orderRepository)
    : IQueryHandler<GetOrdersQuery, GetOrdersQueryResult>
{
    public Task<GetOrdersQueryResult> Query(GetOrdersQuery query, CancellationToken cancellationToken = default)
    {
        var result = orderRepository.GetOrders(query.ToGetOrdersDto());

        return Task.FromResult(new GetOrdersQueryResult { Items = result });
    }
}