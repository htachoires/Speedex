using Speedex.Domain.Commons;
using Speedex.Domain.Orders;
using Speedex.Domain.Parcels;
using Speedex.Domain.Products;

namespace Speedex.Domain.Returns.UseCases.CreateReturn;

public record CreateReturnCommand : ICommand
{
    public ParcelId ParcelId { get; init; }
    public OrderId OrderId { get; init; }
    public IEnumerable<ReturnProductCreateReturnCommand> Products { get; init; }

    public record ReturnProductCreateReturnCommand
    {
        public ProductId ProductId { get; init; }
        public int Quantity { get; init; }
    }
}