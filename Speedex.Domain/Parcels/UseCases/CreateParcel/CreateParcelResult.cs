using Speedex.Domain.Commons;

namespace Speedex.Domain.Parcels.UseCases.CreateParcel;

public record CreateParcelResult : ICommandResult
{
    public ParcelId ParcelId { get; init; }
    public bool Success { get; init; }
}