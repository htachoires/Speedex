using Speedex.Api.Features.Returns.Requests;
using Speedex.Domain.Returns;
using Speedex.Domain.Returns.UseCases.GetReturns;

namespace Speedex.Api.Features.Returns.Mappers;

public static class GetReturnsQueryParamsMapper
{
    public static GetReturnsQuery ToQuery(this GetReturnsQueryParams queryParams)
    {
        return new GetReturnsQuery
        {
            ReturnId = queryParams.ReturnId is not null ? new ReturnId(queryParams.ReturnId) : null,
            PageIndex = queryParams.PageIndex,
            PageSize = queryParams.PageSize,
        };
    }
}