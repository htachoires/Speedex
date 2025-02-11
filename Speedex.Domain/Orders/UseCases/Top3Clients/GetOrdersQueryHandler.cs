using Speedex.Domain.Commons;
using Speedex.Domain.Orders.Repositories;

namespace Speedex.Domain.Orders.UseCases.Top3Clients;

public class GetTop3ClientsQueryHandler(IOrderRepository orderRepository)
    : IQueryHandler<GetTop3ClientsQuery, GetTop3ClientsResult>
{
    public Task<GetTop3ClientsResult> Query(GetTop3ClientsQuery command, CancellationToken cancellationToken = default)
    {
        //TODO: US-11 Compléter la route de statistiques pour récupérer
        // le top 3 des clients qui ont le plus dépensé 🥇🥈🥉

        var result = new GetTop3ClientsResult
        {
            First = new GetTop3ClientsResult.ClientResult(),
            Second = new GetTop3ClientsResult.ClientResult(),
            Third = new GetTop3ClientsResult.ClientResult()
        };

        return Task.FromResult(result);
    }
}