namespace Speedex.Domain.Orders.Repositories.Dtos;

public record GetOrdersDto
{
    public OrderId? OrderId { get; init; }
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
}