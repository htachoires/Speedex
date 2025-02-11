namespace Speedex.Api.Tests.Integration.Features.Returns.Request;

public class CreateReturnTestBodyRequest
{
    public string? ParcelId { get; init; }
    public string? OrderId { get; init; }
    public IEnumerable<CreateReturnTestBodyRequestReturnProduct>? Products { get; init; }
         
    public record CreateReturnTestBodyRequestReturnProduct
    {
        public string? ProductId { get; init; }
        public int? Quantity { get; init; }
    }
}