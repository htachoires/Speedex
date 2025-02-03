using FluentValidation;
using Speedex.Domain.Orders;
using Speedex.Domain.Parcels;
using Speedex.Domain.Products;

namespace Speedex.Domain.Returns.UseCases.CreateReturn;

public class CreateReturnCommandValidator : AbstractValidator<CreateReturnCommand>
{
    public CreateReturnCommandValidator(
        IValidator<ProductId> productIdValidator,
        IValidator<ParcelId> parcelIdValidator,
        IValidator<OrderId> orderIdValidator)
    {
        RuleFor(x => x.Products.Select(p => p.ProductId))
            .ForEach(x => x.SetValidator(productIdValidator));

        RuleFor(x => x.OrderId)
            .SetValidator(orderIdValidator);

        RuleFor(x => x.ParcelId)
            .SetValidator(parcelIdValidator);
    }
}