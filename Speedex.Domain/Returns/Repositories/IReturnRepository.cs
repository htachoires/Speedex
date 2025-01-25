using Speedex.Domain.Returns.Repositories.Dtos;

namespace Speedex.Domain.Returns.Repositories;

public interface IReturnRepository
{
    public UpsertReturnResult UpsertReturn(Return @return);
    public IEnumerable<Return> GetReturns(GetReturnsDto query);
}