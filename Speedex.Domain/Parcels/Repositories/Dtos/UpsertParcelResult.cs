namespace Speedex.Domain.Parcels.Repositories.Dtos;

public record UpsertParcelResult
{
    public UpsertStatus Status { get; init; }

    public enum UpsertStatus
    {
        Success,
        Failed,
    }
}