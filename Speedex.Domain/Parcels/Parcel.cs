using Speedex.Domain.Orders;
using Speedex.Domain.Products;

namespace Speedex.Domain.Parcels;

public record Parcel
{
    public ParcelId ParcelId { get; init; }
    public ParcelStatus ParcelStatus { get; init; }
    public OrderId OrderId { get; init; }
    public IEnumerable<ParcelProduct> Products { get; init; }
    public DateTime CreationDate { get; init; }
    public DateTime UpdateDate { get; init; }
}

public record ParcelId(string Value);

public enum ParcelStatus
{
    Preparing,
    Sending,
    Delivered,
    Lost
}

public record ParcelProduct
{
    public ProductId ProductId { get; init; }
    public int Quantity { get; init; }
}