using Speedex.Api.Features.Returns.Requests;
using Speedex.Domain.Orders;
using Speedex.Domain.Parcels;
using Speedex.Domain.Products;
using Speedex.Domain.Returns.UseCases.CreateReturn;

namespace Speedex.Api.Features.Returns.Mappers;

public static class CreateReturnBodyRequestMapper
{
    public static CreateReturnCommand ToCommand(this CreateReturnBodyRequest bodyRequest)
    {
        return new CreateReturnCommand
        {
            ParcelId = new ParcelId(bodyRequest.ParcelId),
            OrderId = new OrderId(bodyRequest.OrderId),
            Products = bodyRequest.Products.Select(x => new CreateReturnCommand.ReturnProductCreateReturnCommand
            {
                ProductId = new ProductId(x.ProductId),
                Quantity = x.Quantity.Value
            })
        };
    }
}