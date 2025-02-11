using System.Net;
using System.Text.Json;
using Speedex.Api.Features.Products.Requests;
using Speedex.Api.Features.Products.Responses;
using Speedex.Domain.Products;

namespace Speedex.Api.Tests.Integration.Features.Products;

public class GetProductTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public GetProductTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetProductByCategory_Should_Return_ProductsWithTheCategory()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //using a builder would be better but not sure how to do it with CreateProductBodyRequest
        //var product = AProduct.WithCategory("fooDescription").Build();
        var bodyRequestPost = new CreateProductBodyRequest
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
        var createResponse = await httpClient.PostAsJsonAsync("/Products", bodyRequestPost);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        bodyRequestPost = new CreateProductBodyRequest
        {
            Name = "fooName",
            Description = "fooDescription",
            Category = "fooCategory2",
            SecondLevelCategory = "fooSecondLevelCategory2",
            ThirdLevelCategory = "fooThirdLevelCategory2",
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
        createResponse = await httpClient.PostAsJsonAsync("/Products", bodyRequestPost);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        
        var bodyRequest = new GetProductsQueryParams()
        {
            Category = "fooCategory",
        };

        //Act
        var response = await httpClient.GetAsync("/Products?Category=fooCategory");

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var getContent = await response.Content.ReadAsStringAsync();
        var getProductResponse = JsonSerializer.Deserialize<GetProductsResponse>(getContent, _jsonSerializerOptions);

        Assert.NotNull(getProductResponse);
        Assert.NotEmpty(getProductResponse.Items);
        Assert.All(getProductResponse.Items, p => Assert.Equal("fooCategory", p.Category));
    }
}