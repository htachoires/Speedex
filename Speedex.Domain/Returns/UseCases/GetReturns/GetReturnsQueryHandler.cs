using Speedex.Domain.Commons;
using Speedex.Domain.Returns.Repositories;

namespace Speedex.Domain.Returns.UseCases.GetReturns;

public class GetReturnsQueryHandler(IReturnRepository returnRepository)
    : IQueryHandler<GetReturnsQuery, GetReturnsQueryResult>
{
    public GetReturnsQueryResult Query(GetReturnsQuery query)
    {
        var result = returnRepository.GetReturns(query.ToGetReturnsDto());

        return new GetReturnsQueryResult
        {
            Items = result
        };
    }
}