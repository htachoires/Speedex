using Speedex.Domain.Commons;
using Speedex.Domain.Returns.Repositories;

namespace Speedex.Domain.Returns.UseCases.GetReturns;

public class GetReturnsQueryHandler(IReturnRepository returnRepository)
    : IQueryHandler<GetReturnsQuery, GetReturnsQueryResult>
{
    public Task<GetReturnsQueryResult> Query(GetReturnsQuery query, CancellationToken cancellationToken = default)
    {
        var result = returnRepository.GetReturns(query.ToGetReturnsDto());

        return Task.FromResult(new GetReturnsQueryResult { Items = result });
    }
}