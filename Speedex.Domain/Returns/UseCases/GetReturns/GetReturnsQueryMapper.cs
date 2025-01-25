using Speedex.Domain.Returns.Repositories.Dtos;

namespace Speedex.Domain.Returns.UseCases.GetReturns;

public static class GetReturnsQueryMapper
{
    public static GetReturnsDto ToGetReturnsDto(this GetReturnsQuery query)
    {
        const int defaultPageIndex = 1;
        const int defaultPageSize = 100;

        return new GetReturnsDto
        {
            ReturnId = query.ReturnId,
            PageIndex = query.PageIndex ?? defaultPageIndex,
            PageSize = query.PageSize ?? defaultPageSize
        };
    }
}