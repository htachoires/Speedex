using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Speedex.Api.Features.Orders.Mappers;
using Speedex.Api.Features.Orders.Requests;
using Speedex.Domain.Commons;
using Speedex.Domain.Orders.UseCases.CreateOrder;
using Speedex.Domain.Orders.UseCases.GetOrders;

namespace Speedex.Api.Features.Orders;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateOrder([FromBody] CreateOrderBodyRequest bodyRequest,
        [FromServices] ICommandHandler<CreateOrderCommand, CreateOrderResult> handler,
        [FromServices] IValidator<CreateOrderBodyRequest> validator)
    {
        var validationResult = validator.Validate(bodyRequest);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var commandResult = handler.Handle(bodyRequest.ToCommand());

        return Created(commandResult.OrderId.Value, null);
    }

    [HttpGet]
    public IActionResult GetOrders([FromQuery] GetOrdersQueryParams queryParams,
        [FromServices] IQueryHandler<GetOrdersQuery, GetOrdersQueryResult> handler,
        [FromServices] IValidator<GetOrdersQueryParams> validator)
    {
        var validationResult = validator.Validate(queryParams);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = handler.Query(queryParams.ToQuery());

        return Ok(result.Items);
    }
}