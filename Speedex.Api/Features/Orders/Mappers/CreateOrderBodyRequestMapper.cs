using Speedex.Api.Features.Orders.Requests;
using Speedex.Domain.Orders;
using Speedex.Domain.Orders.UseCases.CreateOrder;
using Speedex.Domain.Products;

namespace Speedex.Api.Features.Orders.Mappers;

public static class CreateOrderBodyRequestMapper
{
    public static CreateOrderCommand ToCommand(this CreateOrderBodyRequest bodyRequest)
    {
        return new CreateOrderCommand
        {
            Products = bodyRequest.Products!.Select(
                x => new CreateOrderCommand.Product
                {
                    ProductId = new ProductId(x.ProductId!),
                    Quantity = x.Quantity!.Value,
                }),
            DeliveryType = Enum.Parse<DeliveryType>(bodyRequest.DeliveryType!, true),
            Recipient = new CreateOrderCommand.CreateOrderRecipient
            {
                FirstName = bodyRequest.Recipient!.FirstName,
                LastName = bodyRequest.Recipient.LastName,
                Email = bodyRequest.Recipient.Email,
                Phone = bodyRequest.Recipient.Phone,
                Address = bodyRequest.Recipient.Address,
                AdditionalAddress = bodyRequest.Recipient.AdditionalAddress,
                City = bodyRequest.Recipient.City,
                Country = bodyRequest.Recipient.Country
            }
        };
    }
}