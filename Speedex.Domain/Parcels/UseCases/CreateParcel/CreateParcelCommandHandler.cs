using FluentValidation;
using Speedex.Domain.Commons;
using Speedex.Domain.Parcels.Repositories;
using Speedex.Domain.Parcels.Repositories.Dtos;
using Speedex.Domain.Orders.Repositories; // Assurez-vous d'avoir une référence au repository des commandes
using Speedex.Domain.Orders.Repositories.Dtos; // Pour accéder aux commandes

namespace Speedex.Domain.Parcels.UseCases.CreateParcel;

public class CreateParcelCommandHandler : ICommandHandler<CreateParcelCommand, CreateParcelResult>
{
    private readonly IParcelRepository _parcelRepository;
    private readonly IOrderRepository _orderRepository;  // Ajout du repository des commandes
    private readonly IValidator<CreateParcelCommand> _commandValidator;

    public CreateParcelCommandHandler(IParcelRepository parcelRepository, IValidator<CreateParcelCommand> commandValidator)
        : this(parcelRepository, commandValidator, null)
    {
    }

    public CreateParcelCommandHandler(
        IParcelRepository parcelRepository,
        IValidator<CreateParcelCommand> commandValidator,
        IOrderRepository? orderRepository)
    {
        parcelRepository = parcelRepository ?? throw new ArgumentNullException(nameof(parcelRepository));
        _commandValidator = commandValidator ?? throw new ArgumentNullException(nameof(commandValidator));
        _orderRepository = orderRepository;
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

        var order = await _orderRepository.GetOrderById(command.OrderId, cancellationToken);
        if (order == null)
        {
            return new CreateParcelResult
            {
                Success = false,
                Errors = new List<CreateParcelResult.ValidationError>
                {
                    new CreateParcelResult.ValidationError
                    {
                        Message = "Order not found",
                        PropertyName = "OrderId",
                        Code = "Order_Not_Found"
                    }
                }
            };
        }

        foreach (var parcelProduct in command.Products)
        {
            var orderProduct = order.Products.FirstOrDefault(p => p.ProductId == parcelProduct.ProductId);
            if (orderProduct != null)
            {
                if (parcelProduct.Quantity > orderProduct.Quantity)
                {
                    return new CreateParcelResult
                    {
                        Success = false,
                        Errors = new List<CreateParcelResult.ValidationError>
                        {
                            new CreateParcelResult.ValidationError
                            {
                                Message = $"Quantity for product {parcelProduct.ProductId} exceeds the ordered quantity",
                                PropertyName = "Products",
                                Code = "Quantity_Exceeded"
                            }
                        }
                    };
                }
            }
            else
            {
                return new CreateParcelResult
                {
                    Success = false,
                    Errors = new List<CreateParcelResult.ValidationError>
                    {
                        new CreateParcelResult.ValidationError
                        {
                            Message = $"Product {parcelProduct.ProductId} not found in the order",
                            PropertyName = "Products",
                            Code = "Product_Not_Found"
                        }
                    }
                };
            }
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
