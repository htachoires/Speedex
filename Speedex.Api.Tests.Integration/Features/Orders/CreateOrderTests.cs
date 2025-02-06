using System.Net;
using System.Text;
using System.Text.Json;
using Speedex.Api.Features.Orders.Requests;
using Speedex.Api.Features.Orders.Responses;
using Speedex.Api.Tests.Integration.Features.Orders.Requests;
using Speedex.Domain.Orders;
using Speedex.Domain.Products.Repositories;

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
    public async Task CreateOrder_Should_ReturnCreatedStatusCode_And_FindCreatedOrder()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        var product = AProduct.Build();

        _factory.Services.GetRequiredService<IProductRepository>().UpsertProduct(product);

        var request = new CreateOrderTestBodyRequest
        {
            DeliveryType = DeliveryType.Standard.ToString(),
            Products = [
            new CreateOrderTestBodyRequest.ProductTestBodyRequest
            {
                ProductId = product.ProductId.Value,
                Quantity = 10
            }
            ],
            Recipient = new CreateOrderTestBodyRequest.RecipientTestBodyRequest
            {
                FirstName = "fooFirstName",
                LastName = "fooLastName",
                Email = "fooEmail",
                Phone = "fooPhone",
                Address = "fooAddress",
                AdditionalAddress = "fooAdditionalAddress",
                City = "fooCity",
                Country = "fooCountry"
            }
        };

        // Act
        var response = await httpClient.PostAsync("/Orders", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var location = response.Headers.Location.ToString();

        var getResponse = await httpClient.GetAsync(location);

        var content = await getResponse.Content.ReadAsStringAsync();
        var getOrdersResponse = JsonSerializer.Deserialize<GetOrdersResponse>(content, _jsonSerializerOptions);

        Assert.Single(getOrdersResponse!.Items);
    }

    [Fact]
    public async Task CreateOrder_Should_ReturnBadRequest_When_ProductIsNotFound()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        var productIdNotFound = "productIdNotFound";

        var request = new CreateOrderBodyRequest
        {
            DeliveryType = DeliveryType.Standard.ToString(),
            Products = [
                new CreateOrderBodyRequest.ProductBodyRequest
                {
                    ProductId = productIdNotFound,
                    Quantity = 10
                }
            ],
            Recipient = new CreateOrderBodyRequest.RecipientBodyRequest
            {
                FirstName = "fooFirstName",
                LastName = "fooLastName",
                Email = "fooEmail",
                Phone = "fooPhone",
                Address = "fooAddress",
                AdditionalAddress = "fooAdditionalAddress",
                City = "fooCity",
                Country = "fooCountry"
            }
        };

        // Act
        var response = await httpClient.PostAsync("/Orders", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("IsExistingProductValidator", content);
    }
}