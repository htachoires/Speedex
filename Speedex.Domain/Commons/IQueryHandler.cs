namespace Speedex.Domain.Commons;

public interface IQueryHandler<in TQuery, TQueryResult>
    where TQuery : IQuery
    where TQueryResult : IQueryResult
{
    Task<TQueryResult> Query(TQuery command, CancellationToken cancellationToken = default);
}

public interface IQuery
{
}

public interface IQueryResult
{
}