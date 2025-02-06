using System.Net;
using System.Text.Json;
using Speedex.Api.Features.Products.Requests;
using Speedex.Api.Features.Products.Responses;
using Speedex.Domain.Products;

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
    public async Task CreateProduct_Should_ReturnCreatedStatusCode_And_FindCreatedProduct()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        var bodyRequest = new CreateProductBodyRequest
        {
            Name = "fooName",
            Description = "fooDescription",
            Category = "fooCategory",
            SecondLevelCategory = "fooSecondLevelCategory",
            ThirdLevelCategory = "fooThirdLevelCategory",
            Price = new CreateProductBodyRequest.PriceGetProductBodyRequest
            {
                Amount = 10,
                Currency = Currency.EUR.ToString()
            },
            Dimensions = new CreateProductBodyRequest.DimensionsGetProductBodyRequest
            {
                X = 0.1,
                Y = 0.2,
                Z = 0.3,
                Unit = DimensionUnit.M.ToString()
            },
            Weight = new CreateProductBodyRequest.WeightGetProductBodyRequest
            {
                Value = 1,
                Unit = WeightUnit.Kg.ToString()
            }
        };


        //Act
        var response = await httpClient.PostAsJsonAsync("/Products", bodyRequest);

        //Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var location = response.Headers.Location!.ToString();

        var getResponse = await httpClient.GetAsync(location);

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var getContent = await getResponse.Content.ReadAsStringAsync();
        var getProductResponse = JsonSerializer.Deserialize<GetProductsResponse>(getContent, _jsonSerializerOptions);

        Assert.NotNull(getProductResponse);
        Assert.Single(getProductResponse.Items);
    }
}