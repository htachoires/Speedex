using Speedex.Domain.Commons;
using Speedex.Domain.Parcels.Repositories;
using Speedex.Domain.Parcels.Repositories.Dtos;

namespace Speedex.Domain.Parcels.UseCases.CreateParcel;

public class CreateParcelCommandHandler(IParcelRepository parcelRepository)
    : ICommandHandler<CreateParcelCommand, CreateParcelResult>
{
    public CreateParcelResult Handle(CreateParcelCommand command)
    {
        var now = DateTime.Now;

        var newCommand = new Parcel
        {
            ParcelId = new ParcelId(Guid.NewGuid().ToString()),
            ParcelStatus = ParcelStatus.Created,
            OrderId = command.OrderId,
            Products = command.Products.Select(x => new ParcelProduct
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }),
            CreationDate = now,
            UpdateDate = now
        };

        var result = parcelRepository.UpsertParcel(newCommand);

        if (result.Status != UpsertParcelResult.UpsertStatus.Success)
        {
            return new CreateParcelResult
            {
                Success = false
            };
        }

        return new CreateParcelResult
        {
            ParcelId = newCommand.ParcelId,
            Success = true,
        };
    }
}