using Speedex.Domain.Commons;
using Speedex.Domain.Orders.Repositories;

namespace Speedex.Domain.Orders.UseCases.GetOrders;

public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, GetOrdersQueryResult>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public GetOrdersQueryResult Query(GetOrdersQuery query)
    {
        var result = _orderRepository.GetOrders(query.ToGetOrdersDto());

        return new GetOrdersQueryResult
        {
            Items = result
        };
    }
}