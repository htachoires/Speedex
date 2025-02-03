using Speedex.Domain.Commons;

namespace Speedex.Domain.Returns.UseCases.CreateReturn;

public record CreateReturnResult : ICommandResult
{
    public ReturnId ReturnId { get; init; }
    public bool Success { get; init; }
    public List<ValidationError> Errors { get; set; }

    public record ValidationError
    {
        public string Message { get; init; }
        public string PropertyName { get; init; }
        public string Code { get; set; }
    }
}