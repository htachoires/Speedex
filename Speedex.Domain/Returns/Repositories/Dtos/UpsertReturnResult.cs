namespace Speedex.Domain.Returns.Repositories.Dtos;

public record UpsertReturnResult
{
    public UpsertStatus Status { get; init; }

    public enum UpsertStatus
    {
        Success,
        Failed,
    }
}