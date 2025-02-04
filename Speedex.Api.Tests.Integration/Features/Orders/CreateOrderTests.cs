using System.Net;
using System.Text;
using System.Text.Json;
using Speedex.Api.Features.Orders.Responses;

namespace Speedex.Api.Tests.Integration.Features.Orders;

public class CreateOrderTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public CreateOrderTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task TODO_CreateOrder_Should_ReturnCreatedStatusCode_And_FindCreatedOrder()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        //TODO(lvl-1) Setup a request object to create an order
        var request = new { };

        // Act
        var response = await httpClient.PostAsync("/Orders", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var location = response.Headers.Location.ToString();

        var getResponse = await httpClient.GetAsync(location);

        var content = await getResponse.Content.ReadAsStringAsync();
        var getOrdersResponse = JsonSerializer.Deserialize<GetOrdersResponse>(content, _jsonSerializerOptions);

        Assert.Single(getOrdersResponse!.Items);
    }

    [Fact]
    public async Task TODO_CreateOrder_Should_ReturnBadRequest_When_ProductIsNotFound()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        //TODO(lvl-4) Setup a request object to create an order
        var request = new { };

        // Act
        var response = await httpClient.PostAsync("/Orders", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("IsExistingProductValidator", content);
    }
}