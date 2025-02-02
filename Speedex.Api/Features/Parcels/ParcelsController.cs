using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Speedex.Api.Features.Parcels.Mappers;
using Speedex.Api.Features.Parcels.Requests;
using Speedex.Domain.Commons;
using Speedex.Domain.Parcels.UseCases.CreateParcel;
using Speedex.Domain.Parcels.UseCases.GetParcels;

namespace Speedex.Api.Features.Parcels;

[ApiController]
[Route("[controller]")]
public class ParcelsController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateParcel([FromBody] CreateParcelBodyRequest bodyRequest,
        [FromServices] ICommandHandler<CreateParcelCommand, CreateParcelResult> handler,
        [FromServices] IValidator<CreateParcelBodyRequest> validator)
    {
        var validationResult = validator.Validate(bodyRequest);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var commandResult = handler.Handle(bodyRequest.ToCommand());

        return Created($"/Parcels?ParcelId={commandResult.ParcelId.Value}", null);
    }

    [HttpGet]
    public IActionResult GetParcels([FromQuery] GetParcelsQueryParams queryParams,
        [FromServices] IQueryHandler<GetParcelsQuery, GetParcelsQueryResult> handler,
        [FromServices] IValidator<GetParcelsQueryParams> validator)
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