using Speedex.Domain.Commons;

namespace Speedex.Domain.Returns.UseCases.GetReturns;

public record GetReturnsQuery : IQuery
{
    public ReturnId? ReturnId { get; init; }
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
}