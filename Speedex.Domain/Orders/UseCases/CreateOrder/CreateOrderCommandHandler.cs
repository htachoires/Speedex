using FluentValidation;
using Speedex.Domain.Commons;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.Repositories.Dtos;
using Speedex.Domain.Products.Repositories;
using Speedex.Domain.Products;

namespace Speedex.Domain.Orders.UseCases.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IValidator<CreateOrderCommand> _commandValidator;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IValidator<CreateOrderCommand> commandValidator,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _commandValidator = commandValidator;
        _productRepository = productRepository;

    }

    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken = default)
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

        ;

        var productRepository = _productRepository;
        double totalWeight = 0.0;

        foreach (var p in command.Products)
        {
            Product prod = (await productRepository.GetProductById(p.ProductId, cancellationToken))!;
            if ((prod.Weight.Value > 30.0 && prod.Weight.Unit == WeightUnit.Kg) ||
                (prod.Weight.Value > 30_000.0 && prod.Weight.Unit == WeightUnit.Gr) ||
                (prod.Weight.Value > 30_000_000.0 && prod.Weight.Unit == WeightUnit.Mg))
            {
                return new CreateOrderResult
                {
                    Success = false
                };
            }


        }

        ;



        if (totalWeight > 30.0)
        {
            return new CreateOrderResult
            {
                Success = false
            };
        }

        ;



        var createdOrder = command.ToOrder();
    
        var result = _orderRepository.UpsertOrder(createdOrder);

        if (result.Status != UpsertOrderResult.UpsertStatus.Success)
        {
            return new CreateOrderResult
            {
                Success = false
            };
        }

        ;

        return new CreateOrderResult
        {
            OrderId = createdOrder.OrderId,
            Success = true,
        };
    }
}