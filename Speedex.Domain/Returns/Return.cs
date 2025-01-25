using Speedex.Domain.Orders;
using Speedex.Domain.Parcels;
using Speedex.Domain.Products;

namespace Speedex.Domain.Returns;

public record Return
{
    public ReturnId ReturnId { get; init; }
    public ReturnStatus ReturnStatus { get; init; }
    public ParcelId ParcelId { get; init; }
    public OrderId OrderId { get; init; }
    public IEnumerable<ReturnProduct> Products { get; init; }
    public DateTime CreationDate { get; init; }
    public DateTime UpdateDate { get; init; }
}

public record ReturnProduct
{
    public ProductId ProductId { get; init; }
    public int Quantity { get; init; }
}

public enum ReturnStatus
{
    Created,
    Receipt,
    Qualified,
    Archived
}

public record ReturnId(string Value);