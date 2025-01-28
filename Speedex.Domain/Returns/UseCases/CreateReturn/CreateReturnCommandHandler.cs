using Speedex.Domain.Commons;
using Speedex.Domain.Returns.Repositories;
using Speedex.Domain.Returns.Repositories.Dtos;

namespace Speedex.Domain.Returns.UseCases.CreateReturn;

public class CreateReturnCommandHandler(IReturnRepository returnRepository)
    : ICommandHandler<CreateReturnCommand, CreateReturnResult>
{
    public CreateReturnResult Handle(CreateReturnCommand command)
    {
        var createdReturn = command.ToReturn();

        //Tip: we want to mock returnRepository using NSubstitute
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