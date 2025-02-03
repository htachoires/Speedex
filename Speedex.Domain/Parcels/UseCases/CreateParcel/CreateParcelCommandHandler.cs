using FluentValidation;
using Speedex.Domain.Commons;
using Speedex.Domain.Parcels.Repositories;
using Speedex.Domain.Parcels.Repositories.Dtos;

namespace Speedex.Domain.Parcels.UseCases.CreateParcel;

public class CreateParcelCommandHandler : ICommandHandler<CreateParcelCommand, CreateParcelResult>
{
    private readonly IParcelRepository _parcelRepository;
    private readonly IValidator<CreateParcelCommand> _commandValidator;

    public CreateParcelCommandHandler(IParcelRepository parcelRepository, IValidator<CreateParcelCommand> commandValidator)
    {
        _parcelRepository = parcelRepository;
        _commandValidator = commandValidator;
    }

    public async Task<CreateParcelResult> Handle(CreateParcelCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new CreateParcelResult
            {
                Success = false,
                Errors = validationResult.Errors.Select(
                    x => new CreateParcelResult.ValidationError
                    {
                        Message = x.ErrorMessage,
                        PropertyName = x.PropertyName,
                        Code = x.ErrorCode,
                    }).ToList()
            };
        }

        var now = DateTime.Now;

        var createdParcel = new Parcel
        {
            ParcelId = new ParcelId(Guid.NewGuid().ToString()),
            ParcelStatus = ParcelStatus.Created,
            OrderId = command.OrderId,
            Products = command.Products.Select(
                x => new ParcelProduct
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }),
            CreationDate = now,
            UpdateDate = now
        };

        var result = _parcelRepository.UpsertParcel(createdParcel);

        if (result.Status != UpsertParcelResult.UpsertStatus.Success)
        {
            return new CreateParcelResult
            {
                Success = false
            };
        }

        return new CreateParcelResult
        {
            ParcelId = createdParcel.ParcelId,
            Success = true,
        };
    }
}