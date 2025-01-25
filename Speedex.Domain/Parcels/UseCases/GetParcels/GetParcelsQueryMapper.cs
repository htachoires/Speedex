using Speedex.Domain.Parcels.Repositories.Dtos;

namespace Speedex.Domain.Parcels.UseCases.GetParcels;

public static class GetParcelsQueryMapper
{
    public static GetParcelsDto ToGetParcelsDto(this GetParcelsQuery query)
    {
        const int defaultPageIndex = 1;
        const int defaultPageSize = 100;

        return new GetParcelsDto
        {
            ParcelId = query.ParcelId,
            PageIndex = query.PageIndex ?? defaultPageIndex,
            PageSize = query.PageSize ?? defaultPageSize,
        };
    }
}