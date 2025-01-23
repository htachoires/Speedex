using Speedex.Domain.Commons;

namespace Speedex.Domain.Orders.UseCases.GetOrders;

public record GetOrdersQueryResult : IQueryResult
{
    public IEnumerable<Order> Items { get; init; }
}