using Speedex.Api.Features.Parcels.Responses;
using Speedex.Domain.Parcels.UseCases.GetParcels;

namespace Speedex.Api.Features.Parcels.Mappers;

public static class GetParcelsResponseMapper
{
    public static GetParcelsResponse ToResponse(this GetParcelsQueryResult result)
    {
        return new GetParcelsResponse
        {
            Items = result.Items.Select(x => new GetParcelsResponse.GetParcelItemResponse
            {
                ParcelId = x.ParcelId.Value,
                ParcelStatus = x.ParcelStatus.ToString(),
                OrderId = x.OrderId.Value,
                Products = x.Products.Select(p => new GetParcelsResponse.ParcelProductItemResponse()
                {
                    ProductId = p.ProductId.Value,
                    Quantity = p.Quantity,
                }),
                CreationDate = x.CreationDate.ToString("u"),
                UpdateDate = x.UpdateDate.ToString("u"),
            })
        };
    }
}