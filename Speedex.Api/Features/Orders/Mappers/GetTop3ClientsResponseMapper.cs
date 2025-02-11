using Speedex.Api.Features.Orders.Responses;
using Speedex.Domain.Orders.UseCases.GetOrders;
using Speedex.Domain.Orders.UseCases.Top3Clients;

namespace Speedex.Api.Features.Orders.Mappers;

public static class GetTop3ClientsResponseMapper
{
    public static Top3ClientsResponse ToResponse(this GetTop3ClientsResult result)
    {
        return new Top3ClientsResponse
        {
            First = new Top3ClientsResponse.ClientResponse
            {
                Firstname = result.First.Firstname,
                Lastname = result.First.Lastname,
                Amount = result.First.Amount,
                Currency = result.First.Currency
            },
            Second = new Top3ClientsResponse.ClientResponse
            {
                Firstname = result.Second.Firstname,
                Lastname = result.Second.Lastname,
                Amount = result.Second.Amount,
                Currency = result.Second.Currency
            },
            Third = new Top3ClientsResponse.ClientResponse
            {
                Firstname = result.Third.Firstname,
                Lastname = result.Third.Lastname,
                Amount = result.Third.Amount,
                Currency = result.Third.Currency
            }
        };
    }
}

public record Top3ClientsResponse
{
    public ClientResponse First { get; init; }
    public ClientResponse Second { get; init; }
    public ClientResponse Third { get; init; }

    public record ClientResponse
    {
        public string Firstname { get; init; }
        public string Lastname { get; init; }
        public string Amount { get; init; }
        public string Currency { get; init; }
    }
}