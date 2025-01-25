using Speedex.Domain.Commons;

namespace Speedex.Domain.Returns.UseCases.GetReturns;

public record GetReturnsQueryResult : IQueryResult
{
    public IEnumerable<Return> Items { get; init; }
}