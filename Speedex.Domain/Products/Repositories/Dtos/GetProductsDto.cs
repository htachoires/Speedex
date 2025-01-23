using Speedex.Domain.Orders;

namespace Speedex.Domain.Products.Repositories.Dtos;

public record GetProductsDto
{
    public ProductId? ProductId { get; init; }
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
}