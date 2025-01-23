using Speedex.Domain.Commons;

namespace Speedex.Domain.Orders.UseCases.GetOrders;

public record GetOrdersQuery : IQuery
{
    public OrderId? OrderId { get; init; }
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
}