using Domain.Parcels;
using Domain.Products;

namespace Domain.Returns;

public record Return
{
    public ReturnId ReturnId { get; init; }
    public ReturnStatus Status { get; init; }
    public ParcelId OriginalParcelId { get; init; }
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
    Receipt,
    Qualified,
    Archived
}

public record ReturnId(string Value);