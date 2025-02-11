using Speedex.Domain.Products;

namespace Speedex.Domain.Orders.Repositories.Dtos;

public record GetOrdersDto
{
    public OrderId? OrderId { get; init; }
    public ProductId? ProductId { get; init; }
    public int PageIndex { get; init; }
    public string CustomerEmail { get; init; }
    public int PageSize { get; init; }
}