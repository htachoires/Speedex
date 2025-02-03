using Speedex.Domain.Commons;
using Speedex.Domain.Parcels.Repositories;

namespace Speedex.Domain.Parcels.UseCases.GetParcels;

public class GetParcelsQueryHandler(IParcelRepository parcelRepository)
    : IQueryHandler<GetParcelsQuery, GetParcelsQueryResult>
{
    public Task<GetParcelsQueryResult> Query(GetParcelsQuery query, CancellationToken cancellationToken = default)
    {
        var result = parcelRepository.GetParcels(query.ToGetParcelsDto());

        return Task.FromResult(new GetParcelsQueryResult { Items = result });
    }
}