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
    public async Task<IActionResult> CreateProduct(
        [FromBody] CreateProductBodyRequest bodyRequest,
        [FromServices] ICommandHandler<CreateProductCommand, CreateProductResult> handler,
        [FromServices] IValidator<CreateProductBodyRequest> validator)
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

        return Created($"/Products?ProductId={commandResult.ProductId.Value}", null);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] GetProductsQueryParams queryParams,
        [FromServices] IQueryHandler<GetProductsQuery, GetProductsQueryResult> handler,
        [FromServices] IValidator<GetProductsQueryParams> validator)
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