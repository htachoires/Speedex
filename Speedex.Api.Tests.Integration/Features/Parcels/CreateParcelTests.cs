using System.Net;
using System.Text;
using System.Text.Json;
using Speedex.Api.Features.Parcels.Requests;
using Speedex.Api.Features.Parcels.Responses;
using Speedex.Api.Tests.Integration.Features.Parcels.Request;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Products.Repositories;

namespace Speedex.Api.Tests.Integration.Features.Parcels;

public class CreateParcelTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public CreateParcelTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task TODO_CreateParcel_Should_ReturnCreatedStatusCode_And_FindCreatedParcel()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        var order = AnOrder.Build();
        var product = AProduct.Build();
        
        _factory.Services.GetRequiredService<IOrderRepository>().UpsertOrder(order);
        _factory.Services.GetRequiredService<IProductRepository>().UpsertProduct(product);
        
        var request = new CreateParcelTestBodyRequest
        {
            OrderId = order.OrderId.Value,
            Products =
            [
                new CreateParcelTestBodyRequest.ParcelProductCreateParcelTestBodyRequest()
                {
                    ProductId = product.ProductId.Value,
                    Quantity = 1
                }
            ]
        };

        var bodyContent = JsonSerializer.Serialize(request);

        // Act
        var response = await httpClient.PostAsync("/Parcels", new StringContent(bodyContent, Encoding.UTF8, "application/json"));
        
        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var location = response.Headers.Location.ToString();
        
        var getResponse = await httpClient.GetAsync(location);
        
        var content = await getResponse.Content.ReadAsStringAsync();
        var getParcelsResponse = JsonSerializer.Deserialize<GetParcelsResponse>(content, _jsonSerializerOptions);
        
        Assert.Single(getParcelsResponse!.Items);
    }

    [Fact]
    public async Task TODO_CreateParcel_Should_ReturnBadRequest_When_ProductIdIsNotFound()
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        var order = AnOrder.Build();
        
        _factory.Services.GetRequiredService<IOrderRepository>().UpsertOrder(order);
        
        var request = new CreateParcelTestBodyRequest
        {
            OrderId = order.OrderId.Value,
            Products =
            [
                new CreateParcelTestBodyRequest.ParcelProductCreateParcelTestBodyRequest()
                {
                    ProductId = Guid.NewGuid().ToString(),
                    Quantity = 1
                }
            ]
        };

        var bodyContent = JsonSerializer.Serialize(request);

        // Act
        var response = await httpClient.PostAsync("/Parcels", new StringContent(bodyContent, Encoding.UTF8, "application/json"));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("IsExistingProductValidator", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task TODO_CreateParcel_Should_ReturnBadRequest_When_OrderIdIsNotFound()
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        var product = AProduct.Build();
        
        _factory.Services.GetRequiredService<IProductRepository>().UpsertProduct(product);
        var request = new CreateParcelTestBodyRequest
        {
            OrderId = Guid.NewGuid().ToString(),
            Products =
            [
                new CreateParcelTestBodyRequest.ParcelProductCreateParcelTestBodyRequest()
                {
                    ProductId = product.ProductId.Value,
                    Quantity = 1
                }
            ]
        };

        var bodyContent = JsonSerializer.Serialize(request);

        // Act
        var response = await httpClient.PostAsync("/Parcels", new StringContent(bodyContent, Encoding.UTF8, "application/json"));
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("IsExistingOrderValidator", await response.Content.ReadAsStringAsync());
    }
}