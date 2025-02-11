namespace Speedex.Api.Features.Returns.Requests;

public record CreateReturnBodyRequest
{
    public string? ParcelId { get; init; }
             public string? OrderId { get; init; }
             public IEnumerable<CreateReturnBodyRequestReturnProduct>? Products { get; init; }
         
             public record CreateReturnBodyRequestReturnProduct
             {
                 public string? ProductId { get; init; }
                 public int? Quantity { get; init; }
    }
}