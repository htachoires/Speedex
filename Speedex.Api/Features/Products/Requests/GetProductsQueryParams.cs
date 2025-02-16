namespace Speedex.Api.Features.Products.Requests;

public record GetProductsQueryParams
{
    public string? ProductId { get; init; }
    public int? PageIndex { get; init; }
    public int? PageSize { get; init; }
    public string? Category { get; init; }
}