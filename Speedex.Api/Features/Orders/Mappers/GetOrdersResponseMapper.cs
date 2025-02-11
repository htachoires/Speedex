using Speedex.Api.Features.Orders.Responses;
using Speedex.Domain.Orders.UseCases.GetOrders;

namespace Speedex.Api.Features.Orders.Mappers;

public static class GetOrdersResponseMapper
{
    public static GetOrdersResponse ToResponse(this GetOrdersQueryResult result)
    {
        return new GetOrdersResponse
        {
            Items = result.Items.Select(x => new GetOrdersResponse.GetOrderItemResponse
            {
                OrderId = x.OrderId.Value,
                Products = x.Products.Select(p => new GetOrdersResponse.OrderProductResponse
                {
                    ProductId = p.ProductId.Value,
                    Quantity = p.Quantity
                }),
                DeliveryType = x.DeliveryType.ToString(),
                Recipient = new GetOrdersResponse.Recipient
                {
                    FirstName = x.Recipient.FirstName,
                    LastName = x.Recipient.LastName,
                    Email = x.Recipient.Email,
                    PhoneNumber = x.Recipient.PhoneNumber,
                    Address = x.Recipient.Address,
                    AdditionalAddress = x.Recipient.AdditionalAddress,
                    City = x.Recipient.City,
                    Country = x.Recipient.Country
                },
                CreationDate = x.CreationDate.ToString("u"),
                UpdateDate = x.UpdateDate.ToString("u"),
                TotalAmount = new GetOrdersResponse.GetOrderItemResponse.Price
                {
                    Amount = x.TotalAmount.Amount,
                    Currency = x.TotalAmount.Currency.ToString()
                }
            })
        };
    }
}