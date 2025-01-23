namespace Speedex.Domain.Products.Repositories.Dtos;

public record UpsertProductResult
{
    public UpsertStatus Status { get; init; }

    public enum UpsertStatus
    {
        Success,
        Failed,
    }
}