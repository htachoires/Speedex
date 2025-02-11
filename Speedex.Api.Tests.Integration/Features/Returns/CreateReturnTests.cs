using System.Net;
using System.Text;
using System.Text.Json;
using Speedex.Api.Features.Returns.Requests;
using Speedex.Api.Features.Returns.Responses;
using Speedex.Api.Tests.Integration.Features.Returns.Request;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Parcels.Repositories;
using Speedex.Domain.Products.Repositories;

namespace Speedex.Api.Tests.Integration.Features.Returns;

public class CreateReturnTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public CreateReturnTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateReturn_Should_ReturnCreatedStatusCode_And_FindCreatedReturn()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        var order = AnOrder.Build();
        var parcel = AParcel.Build();
        var product = AProduct.Build();

        _factory.Services.GetRequiredService<IOrderRepository>().UpsertOrder(order);
        _factory.Services.GetRequiredService<IParcelRepository>().UpsertParcel(parcel);
        _factory.Services.GetRequiredService<IProductRepository>().UpsertProduct(product);

        var request = new CreateReturnBodyRequest
        {
            OrderId = order.OrderId.Value,
            ParcelId = parcel.ParcelId.Value,
            Products =
            [
                new CreateReturnBodyRequest.CreateReturnBodyRequestReturnProduct()
                {
                    ProductId = product.ProductId.Value,
                    Quantity = 1
                }
            ]
        };

        // Act
        var response = await httpClient.PostAsync("/Returns", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var location = response.Headers.Location.ToString();

        var getResponse = await httpClient.GetAsync(location);

        var content = await getResponse.Content.ReadAsStringAsync();
        var getReturnsResponse = JsonSerializer.Deserialize<GetReturnsResponse>(content, _jsonSerializerOptions);

        Assert.Single(getReturnsResponse!.Items);
    }

    [Fact]
    public async Task CreateReturn_Should_ReturnBadRequest_When_ParcelIsNotFound()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        var request = new CreateReturnTestBodyRequest
        {
            OrderId = Guid.NewGuid().ToString(),
            ParcelId = Guid.NewGuid().ToString(),
            Products =
            [
                new CreateReturnTestBodyRequest.CreateReturnTestBodyRequestReturnProduct()
                {
                    ProductId = Guid.NewGuid().ToString(),
                    Quantity = 1
                }
            ]
        };

        // Act
        var response = await httpClient.PostAsync("/Returns", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("IsExistingParcelValidator", content);
    }

    [Fact]
    public async Task CreateReturn_Should_ReturnBadRequest_When_ProductIsNotFound()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        var request = new CreateReturnTestBodyRequest
        {
            OrderId = Guid.NewGuid().ToString(),
            ParcelId = Guid.NewGuid().ToString(),
            Products =
            [
                new CreateReturnTestBodyRequest.CreateReturnTestBodyRequestReturnProduct()
                {
                    ProductId = Guid.NewGuid().ToString(),
                    Quantity = 1
                }
            ]
        };

        // Act
        var response = await httpClient.PostAsync("/Returns", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("IsExistingProductValidator", content);
    }

    [Fact]
    public async Task CreateReturn_Should_ReturnBadRequest_When_OrderIsNotFound()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        var request = new CreateReturnTestBodyRequest
        {
            OrderId = Guid.NewGuid().ToString(),
            ParcelId = Guid.NewGuid().ToString(),
            Products =
            [
                new CreateReturnTestBodyRequest.CreateReturnTestBodyRequestReturnProduct()
                {
                    ProductId = Guid.NewGuid().ToString(),
                    Quantity = 1
                }
            ]
        };

        // Act
        var response = await httpClient.PostAsync("/Returns", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("IsExistingOrderValidator", content);
    }
}