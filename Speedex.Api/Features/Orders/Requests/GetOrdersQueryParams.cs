namespace Speedex.Api.Features.Orders.Requests;

public record GetOrdersQueryParams
{
    public string? OrderId { get; init; }
    public int? PageIndex { get; init; }
    public int? PageSize { get; init; }
}