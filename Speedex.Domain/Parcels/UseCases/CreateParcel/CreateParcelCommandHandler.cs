using FluentValidation;
using Speedex.Domain.Commons;
using Speedex.Domain.Parcels.Repositories;
using Speedex.Domain.Parcels.Repositories.Dtos;
using Speedex.Domain.Products;
using Speedex.Domain.Products.Repositories;

namespace Speedex.Domain.Parcels.UseCases.CreateParcel;

public class CreateParcelCommandHandler : ICommandHandler<CreateParcelCommand, CreateParcelResult>
{
    private readonly IParcelRepository _parcelRepository;
    private readonly IProductRepository _productRepository;
    private readonly IValidator<CreateParcelCommand> _commandValidator;
    
    public CreateParcelCommandHandler(IParcelRepository parcelRepository, IProductRepository productRepository, IValidator<CreateParcelCommand> commandValidator)
    {
        _parcelRepository = parcelRepository;
        _productRepository = productRepository;
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
        
        // products volume < 1m^3

        var totalVolume = 0.0;
        
        foreach (var parcelProduct in command.Products)
        {
            Product? product = await _productRepository.GetProductById(parcelProduct.ProductId, CancellationToken.None);
            if (product != null)
            {
                totalVolume += product.Dimensions.VolumeInCubicMeter;
            }
        }

        if (totalVolume >= 1)
        {
            return new CreateParcelResult
            {
                Success = false,
                Errors = new List<CreateParcelResult.ValidationError>
                {
                    new CreateParcelResult.ValidationError
                    {
                        Message = "The total volume cannot be more than 1 cubic meter",
                        PropertyName = "Dimensions.VolumeInCubicMeter",
                        Code = "Parcel_VolumeExceeded_Error",
                    }
                }
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