namespace Speedex.Domain.Orders.Repositories.Dtos;

public record UpsertOrderResult
{
    public UpsertStatus Status { get; init; }

    public enum UpsertStatus
    {
        Success,
        Failed,
    }
}