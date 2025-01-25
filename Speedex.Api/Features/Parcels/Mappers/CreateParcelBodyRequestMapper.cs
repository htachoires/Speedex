using Speedex.Api.Features.Parcels.Requests;
using Speedex.Domain.Orders;
using Speedex.Domain.Parcels.UseCases.CreateParcel;
using Speedex.Domain.Products;

namespace Speedex.Api.Features.Parcels.Mappers;

public static class CreateParcelBodyRequestMapper
{
    public static CreateParcelCommand ToCommand(this CreateParcelBodyRequest bodyRequest)
    {
        return new CreateParcelCommand
        {
            OrderId = new OrderId(bodyRequest.OrderId),
            Products = bodyRequest.Products.Select(x => new CreateParcelCommand.ParcelProductCreateParcelCommand
            {
                ProductId = new ProductId(x.ProductId),
                Quantity = x.Quantity.Value
            })
        };
    }
}