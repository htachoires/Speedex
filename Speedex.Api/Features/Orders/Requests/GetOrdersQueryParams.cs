namespace Speedex.Api.Features.Orders.Requests;

public record GetOrdersQueryParams
{
    public string? OrderId { get; init; }
    public string? ProductId { get; init; }
    public int? PageIndex { get; init; }
    public string? CustomerEmail { get; set; } 
    public int? PageSize { get; init; }
}