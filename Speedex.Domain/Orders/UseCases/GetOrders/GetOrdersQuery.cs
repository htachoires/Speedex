using Speedex.Domain.Commons;
using Speedex.Domain.Products;

namespace Speedex.Domain.Orders.UseCases.GetOrders;

public record GetOrdersQuery : IQuery
{
    public OrderId? OrderId { get; init; }
    public ProductId? ProductId { get; init; }
    public int? PageIndex { get; init; }
    public string? CustomerEmail { get; set; } 
    public int? PageSize { get; init; }
}