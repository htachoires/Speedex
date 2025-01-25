using Speedex.Domain.Commons;

namespace Speedex.Domain.Returns.UseCases.CreateReturn;

public record CreateReturnResult : ICommandResult
{
    public ReturnId ReturnId { get; init; }
    public bool Success { get; init; }
}