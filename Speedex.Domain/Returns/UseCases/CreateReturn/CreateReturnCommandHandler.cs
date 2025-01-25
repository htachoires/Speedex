using Speedex.Domain.Commons;
using Speedex.Domain.Returns.Repositories;
using Speedex.Domain.Returns.Repositories.Dtos;

namespace Speedex.Domain.Returns.UseCases.CreateReturn;

public class CreateReturnCommandHandler(IReturnRepository returnRepository)
    : ICommandHandler<CreateReturnCommand, CreateReturnResult>
{
    public CreateReturnResult Handle(CreateReturnCommand command)
    {
        var now = DateTime.Now;

        var createdReturn = new Return
        {
            ReturnId = new ReturnId(Guid.NewGuid().ToString()),
            ReturnStatus = ReturnStatus.Created,
            OrderId = command.OrderId,
            Products = command.Products.Select(
                x => new ReturnProduct
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }),
            CreationDate = now,
            UpdateDate = now
        };

        var result = returnRepository.UpsertReturn(createdReturn);

        if (result.Status != UpsertReturnResult.UpsertStatus.Success)
        {
            return new CreateReturnResult
            {
                Success = false
            };
        }

        return new CreateReturnResult
        {
            ReturnId = createdReturn.ReturnId,
            Success = true,
        };
    }
}