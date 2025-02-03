using FluentValidation;
using Speedex.Domain.Orders.Repositories;

namespace Speedex.Domain.Orders.Validators;

public class IsExistingOrderValidator : AbstractValidator<OrderId>
{
    public IsExistingOrderValidator(IOrderRepository productRepository)
    {
        RuleFor(x => x)
            .MustAsync(async (orderId, cancellationToken) => await productRepository.IsExistingOrder(orderId, cancellationToken))
            .WithErrorCode(nameof(IsExistingOrderValidator))
            .WithMessage("Order with id {PropertyValue} does not exist");
    }
}