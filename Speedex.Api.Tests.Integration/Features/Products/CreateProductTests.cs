using System.Net;
using System.Text;
using System.Text.Json;
using Speedex.Api.Features.Products.Mappers;
using Speedex.Api.Features.Products.Requests;
using Speedex.Api.Features.Products.Responses;
using Speedex.Api.Tests.Integration.Features.Products.Request;
using Speedex.Domain.Products.Repositories;

namespace Speedex.Api.Tests.Integration.Features.Products;

public class CreateProductTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public CreateProductTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task TODO_CreateProduct_Should_ReturnCreatedStatusCode_And_FindCreatedProduct()
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        
        var product = AProduct.Build();
        _factory.Services.GetRequiredService<IProductRepository>().UpsertProduct(product);
        
        var request = new CreateProductTestBodyRequest
        {
            Name = product.Name,
            Description = product.Description,
            Category = product.Category,
            SecondLevelCategory = product.SecondLevelCategory,
            ThirdLevelCategory = product.ThirdLevelCategory,
            Price = new CreateProductTestBodyRequest.PriceGetProductTestBodyRequest
            {
                Amount = product.Price.Amount,
                Currency = product.Price.Currency.ToString()
            },
            Dimensions = new CreateProductTestBodyRequest.DimensionsGetProductTestBodyRequest
            {
                X = product.Dimensions.X,
                Y = product.Dimensions.Y,
                Z = product.Dimensions.Z,
                Unit = product.Dimensions.Unit.ToString()
            },
            Weight = new CreateProductTestBodyRequest.WeightGetProductTestBodyRequest
            {
                Value = product.Weight.Value,
                Unit = product.Weight.Unit.ToString()
            }
        };

        // Act
        var response = await httpClient.PostAsync("/Products", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var location = response.Headers.Location.ToString();
        var getResponse = await httpClient.GetAsync(location);
        var content = await getResponse.Content.ReadAsStringAsync();
        var getProductResponse = JsonSerializer.Deserialize<GetProductsResponse>(content, _jsonSerializerOptions);
        Assert.NotNull(getProductResponse);
    }
}