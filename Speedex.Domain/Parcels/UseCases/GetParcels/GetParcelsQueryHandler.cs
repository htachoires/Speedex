using Speedex.Domain.Commons;
using Speedex.Domain.Parcels.Repositories;

namespace Speedex.Domain.Parcels.UseCases.GetParcels;

public class GetParcelsQueryHandler(IParcelRepository parcelRepository)
    : IQueryHandler<GetParcelsQuery, GetParcelsQueryResult>
{
    public GetParcelsQueryResult Query(GetParcelsQuery query)
    {
        var result = parcelRepository.GetParcels(query.ToGetParcelsDto());

        return new GetParcelsQueryResult
        {
            Items = result
        };
    }
}