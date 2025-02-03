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
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderBodyRequest bodyRequest,
        [FromServices] ICommandHandler<CreateOrderCommand, CreateOrderResult> handler,
        [FromServices] IValidator<CreateOrderBodyRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(bodyRequest);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var commandResult = await handler.Handle(bodyRequest.ToCommand());

        if (!commandResult.Success)
        {
            return BadRequest(commandResult.Errors);
        }

        return Created($"/Orders?OrderId={commandResult.OrderId.Value}", null);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(
        [FromQuery] GetOrdersQueryParams queryParams,
        [FromServices] IQueryHandler<GetOrdersQuery, GetOrdersQueryResult> handler,
        [FromServices] IValidator<GetOrdersQueryParams> validator)
    {
        var validationResult = await validator.ValidateAsync(queryParams);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await handler.Query(queryParams.ToQuery());

        return Ok(result.ToResponse());
    }
}