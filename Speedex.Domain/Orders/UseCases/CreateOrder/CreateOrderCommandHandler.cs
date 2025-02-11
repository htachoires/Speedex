using FluentValidation;
using Speedex.Domain.Commons;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.Repositories.Dtos;
using Speedex.Domain.Products;
using Speedex.Domain.Products.Repositories;

namespace Speedex.Domain.Orders.UseCases.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IValidator<CreateOrderCommand> _commandValidator;
    private readonly IProductRepository _productRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IValidator<CreateOrderCommand> commandValidator)
        : this(orderRepository, commandValidator, null)
    {
    }

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IValidator<CreateOrderCommand> commandValidator,
        IProductRepository? productRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _commandValidator = commandValidator ?? throw new ArgumentNullException(nameof(commandValidator));
        _productRepository = productRepository;
    }

    public async Task<CreateOrderResult> Handle(CreateOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new CreateOrderResult
            {
                Success = false,
                Errors = validationResult.Errors.Select(
                    x => new CreateOrderResult.ValidationError
                    {
                        Message = x.ErrorMessage,
                        PropertyName = x.PropertyName,
                        Code = x.ErrorCode,
                    }).ToList()
            };
        }

        decimal total = 0;
        decimal weightInKg = 0;
        double volumeTotal = 0;
        if (_productRepository != null)
        {
            foreach (var product in command.Products)
            {
                var productDto = await _productRepository.GetProductById(product.ProductId, cancellationToken);
                if (productDto != null && productDto.Weight != null && productDto.Weight.Unit != null)
                {
                    decimal weight = (decimal)productDto.Weight.Value;
                    switch (productDto.Weight.Unit)
                    {
                        case WeightUnit.Kg:
                            weightInKg += weight * product.Quantity;
                            break;
                        case WeightUnit.Gr:
                            weightInKg += (weight / 1000) * product.Quantity; // Convert grams to kilograms
                            break;
                        case WeightUnit.Mg:
                            weightInKg += (weight / 1000000) * product.Quantity; // Convert milligrams to kilograms
                            break;
                    }

                    if (productDto.Price != null)
                    {
                        total += productDto.Price.Amount * product.Quantity;
                    }
                }

                if (productDto != null && productDto.Dimensions != null && productDto.Dimensions.X != null &&
                    productDto.Dimensions.Y != null && productDto.Dimensions.Z != null &&
                    productDto.Dimensions.Unit != null)
                {
                    volumeTotal += productDto.Dimensions.VolumeInCubicMeter;
                }
            }

            command.Price = total;
            command.Weight = weightInKg;

            // Debugging information
            Console.WriteLine($"Total weight in kg: {weightInKg}");

            if (command.Weight > 30)
            {
                return new CreateOrderResult
                {
                    Success = false,
                    Errors = new List<CreateOrderResult.ValidationError>
                    {
                        new CreateOrderResult.ValidationError
                        {
                            Message = "Command weight is more than 30kg",
                            PropertyName = "Weight",
                            Code = "Command_WeightExceeded_Error"
                        }
                    }
                };
            }

            if (volumeTotal > 1)
            {
                return new CreateOrderResult
                {
                    Success = false,
                    Errors = new List<CreateOrderResult.ValidationError>
                    {
                        new CreateOrderResult.ValidationError
                        {
                            Message = "Command volume is more than 1 m³",
                            PropertyName = "Volume",
                            Code = "Command_VolumeExceeded_Error"
                        }
                    }
                };
            }
        }

        var order = command.ToOrder();
        var upsertResult =  _orderRepository.UpsertOrder(order); // Await the async method

        if (upsertResult == null || upsertResult.Status == UpsertOrderResult.UpsertStatus.Failed)
        {
            return new CreateOrderResult
            {
                Success = false,
                Errors = new List<CreateOrderResult.ValidationError>
                {
                    new CreateOrderResult.ValidationError
                    {
                        Message = "Error while saving order",
                        PropertyName = "Order",
                        Code = "Order_Save_Error"
                    }
                }
            };
        }

        return new CreateOrderResult
        {
            Success = true,
            OrderId = order.OrderId
        };
    }
}