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
            .Where(x => (query.ProductId is null || x.Products.Any(p => p.ProductId == query.ProductId)) &&
                        (string.IsNullOrEmpty(query.CustomerEmail) || x.Recipient.Email == query.CustomerEmail)) 
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
    }


    public Task<bool> IsExistingOrder(OrderId orderId, CancellationToken cancellationToken)
    {
        return Task.FromResult(_orders.ContainsKey(orderId));
    }

    public Task<Order?> GetOrderById(string orderId, CancellationToken cancellationToken = default)
    {
        // Use OrderId type for matching, not string (for type safety)
        var orderIdObj = new OrderId(orderId);

        // Using the dictionary to find the order by the key (OrderId)
        var order = _orders.TryGetValue(orderIdObj, out var foundOrder) ? foundOrder : null;

        // Return result wrapped in Task
        return Task.FromResult(order);
    }
}