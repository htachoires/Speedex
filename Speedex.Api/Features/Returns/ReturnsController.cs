using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Speedex.Api.Features.Returns.Mappers;
using Speedex.Api.Features.Returns.Requests;
using Speedex.Domain.Commons;
using Speedex.Domain.Returns.UseCases.CreateReturn;
using Speedex.Domain.Returns.UseCases.GetReturns;

namespace Speedex.Api.Features.Returns;

[ApiController]
[Route("[controller]")]
public class ReturnsController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateReturn([FromBody] CreateReturnBodyRequest bodyRequest,
        [FromServices] ICommandHandler<CreateReturnCommand, CreateReturnResult> handler,
        [FromServices] IValidator<CreateReturnBodyRequest> validator)
    {
        var validationResult = validator.Validate(bodyRequest);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var commandResult = handler.Handle(bodyRequest.ToCommand());

        return Created("/Returns?ReturnId=" + commandResult.ReturnId.Value, null);
    }

    [HttpGet]
    public IActionResult GetReturns([FromQuery] GetReturnsQueryParams queryParams,
        [FromServices] IQueryHandler<GetReturnsQuery, GetReturnsQueryResult> handler,
        [FromServices] IValidator<GetReturnsQueryParams> validator)
    {
        var validationResult = validator.Validate(queryParams);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = handler.Query(queryParams.ToQuery());

        return Ok(result.ToResponse());
    }
}