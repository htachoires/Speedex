using Speedex.Domain.Returns;
using Speedex.Domain.Returns.Repositories;
using Speedex.Domain.Returns.Repositories.Dtos;

namespace Speedex.Infrastructure;

public class InMemoryReturnRepository : IReturnRepository
{
    private readonly Dictionary<ReturnId, Return> _returns = new();

    public UpsertReturnResult UpsertReturn(Return @return)
    {
        if (!_returns.TryAdd(@return.ReturnId, @return))
        {
            _returns[@return.ReturnId] = @return;
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
            return _returns.TryGetValue(query.ReturnId, out var @return) ? new List<Return> { @return } : new List<Return>();
        }

        return _returns.Values
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
    }
}