using Speedex.Domain.Parcels.Repositories.Dtos;

namespace Speedex.Domain.Parcels.UseCases.GetParcels;

public static class GetParcelsQueryMapper
{
    public static GetParcelsDto ToGetParcelsDto(this GetParcelsQuery query)
    {
        return new GetParcelsDto
        {
            ParcelId = query.ParcelId,
            PageIndex = query.PageIndex,
            PageSize = query.PageSize
        };
    }
}