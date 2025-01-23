using Speedex.Domain.Orders.Repositories.Dtos;

namespace Speedex.Domain.Orders.Repositories;

public interface IOrderRepository
{
    public UpsertOrderResult UpsertCommand(Order order);
    public IEnumerable<Order> GetOrders(GetOrdersDto query);
}