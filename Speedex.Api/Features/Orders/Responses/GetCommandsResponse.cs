namespace Speedex.Api.Features.Orders.Responses;

public class GetOrdersResponse
{
    public IEnumerable<GetCommandResponse> Items { get; init; }

    public record GetCommandResponse
    {
        public string CommandId { get; init; }
    }
}