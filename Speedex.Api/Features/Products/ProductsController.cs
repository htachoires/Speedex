using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Speedex.Api.Features.Products.Mappers;
using Speedex.Api.Features.Products.Requests;
using Speedex.Domain.Commons;
using Speedex.Domain.Products.UseCases.CreateProduct;
using Speedex.Domain.Products.UseCases.GetProducts;

namespace Speedex.Api.Features.Products;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateProduct([FromBody] CreateProductBodyRequest bodyRequest,
        [FromServices] ICommandHandler<CreateProductCommand, CreateProductResult> handler,
        [FromServices] IValidator<CreateProductBodyRequest> validator)
    {
        var validationResult = validator.Validate(bodyRequest);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var commandResult = handler.Handle(bodyRequest.ToCommand());

        return Created(commandResult.ProductId.Value, null);
    }

    [HttpGet]
    public IActionResult GetProducts([FromQuery] GetProductsQueryParams queryParams,
        [FromServices] IQueryHandler<GetProductsQuery, GetProductsQueryResult> handler,
        [FromServices] IValidator<GetProductsQueryParams> validator)
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