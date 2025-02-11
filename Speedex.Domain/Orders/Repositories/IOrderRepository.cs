using Speedex.Domain.Orders.Repositories.Dtos;

namespace Speedex.Domain.Orders.Repositories;

public interface IOrderRepository
{
    UpsertOrderResult UpsertOrder(Order order);

    IEnumerable<Order> GetOrders(GetOrdersDto query);

    Task<bool> IsExistingOrder(OrderId orderId, CancellationToken cancellationToken = default);
    Task<Order?> GetOrderById(OrderId orderId, CancellationToken cancellationToken = default);
}