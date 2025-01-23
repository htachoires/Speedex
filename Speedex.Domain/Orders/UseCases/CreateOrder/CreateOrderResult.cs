using Speedex.Domain.Commons;

namespace Speedex.Domain.Orders.UseCases.CreateOrder;

public record CreateOrderResult : ICommandResult
{
    public OrderId OrderId { get; init; }
    public bool Success { get; init; }
}