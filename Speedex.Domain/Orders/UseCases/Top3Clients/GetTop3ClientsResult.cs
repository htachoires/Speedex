using Speedex.Domain.Commons;
using Speedex.Domain.Products;

namespace Speedex.Domain.Orders.UseCases.Top3Clients;

public record GetTop3ClientsResult : IQueryResult
{
    public ClientResult First { get; init; }
    public ClientResult Second { get; init; }
    public ClientResult Third { get; init; }

    public record ClientResult
    {
        public string Firstname { get; init; }
        public string Email { get; init; }
        public string Lastname { get; init; }
        public decimal Amount { get; init; }
        public Currency Currency { get; init; }
    }
}