using Speedex.Domain.Orders;
using Speedex.Domain.Returns.Repositories.Dtos;

namespace Speedex.Domain.Returns.Repositories;

public interface IReturnRepository
{
    public UpsertReturnResult UpsertReturn(Return Return);
    public IEnumerable<Return> GetReturns(GetReturnsDto query);
}