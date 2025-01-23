namespace Speedex.Domain.Commons;

public interface IQueryHandler<in TQuery, out TQueryResult>
    where TQuery : IQuery
    where TQueryResult : IQueryResult
{
    TQueryResult Query(TQuery command);
}

public interface IQuery
{
}

public interface IQueryResult
{
}