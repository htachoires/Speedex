using Speedex.Api.Features.Parcels.Requests;
using Speedex.Domain.Parcels;
using Speedex.Domain.Parcels.UseCases.GetParcels;

namespace Speedex.Api.Features.Parcels.Mappers;

public static class GetParcelsQueryParamsMapper
{
    public static GetParcelsQuery ToQuery(this GetParcelsQueryParams queryParams)
    {
        return new GetParcelsQuery
        {
            ParcelId = queryParams.ParcelId is not null ? new ParcelId(queryParams.ParcelId) : null,
            PageIndex = queryParams.PageIndex,
            PageSize = queryParams.PageSize,
        };
    }
}