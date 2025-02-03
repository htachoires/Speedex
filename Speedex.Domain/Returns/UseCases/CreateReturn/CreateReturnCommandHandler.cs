using FluentValidation;
using Speedex.Domain.Commons;
using Speedex.Domain.Returns.Repositories;
using Speedex.Domain.Returns.Repositories.Dtos;

namespace Speedex.Domain.Returns.UseCases.CreateReturn;

public class CreateReturnCommandHandler : ICommandHandler<CreateReturnCommand, CreateReturnResult>
{
    private readonly IReturnRepository _returnRepository;
    private readonly IValidator<CreateReturnCommand> _commandValidator;

    public CreateReturnCommandHandler(IReturnRepository returnRepository, IValidator<CreateReturnCommand> commandValidator)
    {
        _returnRepository = returnRepository;
        _commandValidator = commandValidator;
    }

    public async Task<CreateReturnResult> Handle(CreateReturnCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new CreateReturnResult
            {
                Success = false,
                Errors = validationResult.Errors.Select(
                    x => new CreateReturnResult.ValidationError
                    {
                        Message = x.ErrorMessage,
                        PropertyName = x.PropertyName,
                        Code = x.ErrorCode,
                    }).ToList()
            };
        }

        var createdReturn = command.ToReturn();

        var result = _returnRepository.UpsertReturn(createdReturn);

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