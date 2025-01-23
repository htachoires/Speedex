using Speedex.Domain.Returns;
using Speedex.Domain.Returns.Repositories;
using Speedex.Domain.Returns.Repositories.Dtos;

namespace Speedex.Infrastructure;

public class InMemoryReturnRepository : IReturnRepository
{
    private readonly Dictionary<ReturnId, Return> _orders = new();

    public UpsertReturnResult UpsertReturn(Return order)
    {
        if (!_orders.TryAdd(order.ReturnId, order))
        {
            _orders[order.ReturnId] = order;
        }

        return new UpsertReturnResult
        {
            Status = UpsertReturnResult.UpsertStatus.Success,
        };
    }

    public IEnumerable<Return> GetReturns(GetReturnsDto query)
    {
        if (query.ReturnId is not null)
        {
            return _orders.TryGetValue(query.ReturnId, out var order) ? new List<Return> { order } : new List<Return>();
        }

        return _orders.Values
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
    }
}