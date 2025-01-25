using Speedex.Domain.Commons;
using Speedex.Domain.Orders;
using Speedex.Domain.Products;

namespace Speedex.Domain.Parcels.UseCases.CreateParcel;

public record CreateParcelCommand : ICommand
{
    public OrderId OrderId { get; init; }
    public IEnumerable<ParcelProductCreateParcelCommand> Products { get; init; }

    public record ParcelProductCreateParcelCommand
    {
        public ProductId ProductId { get; init; }
        public int Quantity { get; init; }
    }
}