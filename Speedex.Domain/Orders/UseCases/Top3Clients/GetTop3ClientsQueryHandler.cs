using Speedex.Domain.Commons;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.Repositories.Dtos;
using Speedex.Domain.Products;

namespace Speedex.Domain.Orders.UseCases.Top3Clients;

public class GetTop3ClientsQueryHandler(IOrderRepository orderRepository)
    : IQueryHandler<GetTop3ClientsQuery, GetTop3ClientsResult>
{
    public Task<GetTop3ClientsResult> Query(GetTop3ClientsQuery command, CancellationToken cancellationToken = default)
    {
        var prices = orderRepository
            .GetOrders(new GetOrdersDto())
            .Aggregate(
                new Dictionary<Recipient, Price>(),
                (dictionary, order) =>
                {
                    var recipientEmail = order.Recipient;
                    if (dictionary.ContainsKey(recipientEmail))
                    {
                        dictionary[recipientEmail] = new Price()
                        {
                            Amount = dictionary[recipientEmail].ToEUR().Amount + order.TotalAmount.ToEUR().Amount,
                            Currency = Currency.EUR
                        };
                    }
                    else
                    {
                        dictionary.Add(recipientEmail, order.TotalAmount);
                    }

                    return dictionary;
                }).OrderBy(pair => pair.Value.Amount)
            .ToDictionary();

        var first = prices.ElementAt(0);
        var second = prices.ElementAt(1);
        var third = prices.ElementAt(2);


        var result = new GetTop3ClientsResult
        {
            First = new GetTop3ClientsResult.ClientResult
            {
                Firstname = first.Key.FirstName,
                Email = first.Key.Email,
                Lastname = first.Key.LastName,
                Amount = first.Value.Amount,
                Currency = first.Value.Currency
            },
            Second = new GetTop3ClientsResult.ClientResult
            {
                Firstname = second.Key.FirstName,
                Email = second.Key.Email,
                Lastname = second.Key.LastName,
                Amount = second.Value.Amount,
                Currency = second.Value.Currency
            },
            Third = new GetTop3ClientsResult.ClientResult
            {
                Firstname = third.Key.FirstName,
                Email = third.Key.Email,
                Lastname = third.Key.LastName,
                Amount = third.Value.Amount,
                Currency = third.Value.Currency
            }
        };

        return Task.FromResult(result);
    }
}