namespace Speedex.Api.Features.Returns.Requests;

public record GetReturnsQueryParams
{
    public string? ReturnId { get; init; }
    public int? PageIndex { get; init; }
    public int? PageSize { get; init; }
}