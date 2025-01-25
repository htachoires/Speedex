using Speedex.Domain.Returns.Repositories.Dtos;

namespace Speedex.Domain.Returns.UseCases.GetReturns;

public static class GetReturnsQueryMapper
{
    public static GetReturnsDto ToGetReturnsDto(this GetReturnsQuery query)
    {
        return new GetReturnsDto
        {
            ReturnId = query.ReturnId,
            PageIndex = query.PageIndex,
            PageSize = query.PageSize
        };
    }
}