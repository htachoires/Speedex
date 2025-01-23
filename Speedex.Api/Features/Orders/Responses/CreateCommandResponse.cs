namespace Speedex.Api.Features.Orders.Responses;

public record CreateOrderResponse
{
    public string CommandId { get; init; }
}