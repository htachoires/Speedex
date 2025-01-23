using Speedex.Domain.Orders;

namespace Speedex.Domain.Returns.Repositories.Dtos;

public record GetReturnsDto
{
    public ReturnId? ReturnId { get; init; }
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
}