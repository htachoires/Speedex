using Speedex.Domain.Orders;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.Repositories.Dtos;

namespace Speedex.Infrastructure;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly Dictionary<OrderId, Order> _orders = new();

    public UpsertOrderResult UpsertOrder(Order order)
    {
        if (!_orders.TryAdd(order.OrderId, order))
        {
            _orders[order.OrderId] = order;
        }

        return new UpsertOrderResult
        {
            Status = UpsertOrderResult.UpsertStatus.Success,
        };
    }

    public IEnumerable<Order> GetOrders(GetOrdersDto query)
    {
        if (query.OrderId is not null)
        {
            return _orders.TryGetValue(query.OrderId, out var order) ? new List<Order> { order } : new List<Order>();
        }

        return _orders.Values
            .Where(x => query.ProductId is null || x.Products.Any(p => p.ProductId == query.ProductId))
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
    }

    public Task<bool> IsExistingOrder(OrderId orderId, CancellationToken cancellationToken)
    {
        return Task.FromResult(_orders.ContainsKey(orderId));
    }
}