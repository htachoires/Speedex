using System.Net;
using System.Text;
using System.Text.Json;
using Speedex.Api.Features.Orders.Requests;
using Speedex.Api.Features.Orders.Responses;
using Speedex.Api.Tests.Integration.Features.Orders.Request;
using Speedex.Domain.Orders;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.UseCases.CreateOrder;
using Speedex.Domain.Parcels.Repositories;
using Speedex.Domain.Products;
using Speedex.Domain.Products.Repositories;
using Speedex.Tests.Tools.TestDataBuilders.Domain.Orders;

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

        var product = AProduct.Build();
        var recipient = RecipientBuilder.ARecipient.Build();
        
        _factory.Services.GetRequiredService<IProductRepository>().UpsertProduct(product);
        
        var request = new CreateOrderTestBodyRequest
        {   
            DeliveryType = DeliveryType.Standard.ToString(),
            Products =
            [
                new CreateOrderTestBodyRequest.ProductTestBodyRequest()
                {
                    ProductId = product.ProductId.Value,
                    Quantity = 1
                }
            ],
            Recipient = new CreateOrderTestBodyRequest.RecipientTestBodyRequest()
            {
                FirstName = recipient.FirstName,
                LastName = recipient.LastName,
                Email = recipient.Email,
                Address = recipient.Address,
                Phone = recipient.PhoneNumber,
                AdditionalAddress = recipient.AdditionalAddress,
                City = recipient.City,
                Country = recipient.Country
            }
        };

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

        var recipient = RecipientBuilder.ARecipient.Build();
        
        
        var request = new CreateOrderTestBodyRequest()
        {
            DeliveryType = DeliveryType.Standard.ToString(),
            Products =
            [
                new CreateOrderTestBodyRequest.ProductTestBodyRequest()
                {
                    ProductId = Guid.NewGuid().ToString(),
                    Quantity = 1
                }
            ],
            Recipient = new CreateOrderTestBodyRequest.RecipientTestBodyRequest()
            {
                FirstName = recipient.FirstName,
                LastName = recipient.LastName,
                Email = recipient.Email,
                Address = recipient.Address,
                Phone = recipient.PhoneNumber,
                AdditionalAddress = recipient.AdditionalAddress,
                City = recipient.City,
                Country = recipient.Country
            }
        };

        // Act
        var response = await httpClient.PostAsync("/Orders", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("IsExistingProductValidator", content);
    }

    [Fact]
    public async Task CreateOrder_With_Multiple_Products_Should_ReturnCreatedStatusCode_And_FindCreatedOrder_And_Have_Correct_TotalPrice()
    {
        // Arange 
        var httpClient = _factory.CreateClient();
        
        var product1 = AProduct.WithPrice(50, Currency.USD).Build();
        var product2 = AProduct.WithPrice(10, Currency.USD).Build();
        var recipient = RecipientBuilder.ARecipient.Build();
        
        _factory.Services.GetRequiredService<IProductRepository>().UpsertProduct(product1);
        _factory.Services.GetRequiredService<IProductRepository>().UpsertProduct(product2);

        var request = new CreateOrderTestBodyRequest()
        {
            DeliveryType = DeliveryType.Standard.ToString(),
            Products =
            [
                new CreateOrderTestBodyRequest.ProductTestBodyRequest()
                {
                    ProductId = product1.ProductId.Value,
                    Quantity = 1
                },
                new CreateOrderTestBodyRequest.ProductTestBodyRequest()
                {
                    ProductId = product2.ProductId.Value,
                    Quantity = 2
                }
            ],
            Recipient = new CreateOrderTestBodyRequest.RecipientTestBodyRequest()
            {
                FirstName = recipient.FirstName,
                LastName = recipient.LastName,
                Email = recipient.Email,
                Address = recipient.Address,
                Phone = recipient.PhoneNumber,
                AdditionalAddress = recipient.AdditionalAddress,
                City = recipient.City,
                Country = recipient.Country
            }
        };
        
        
        // Act
        var response = await httpClient.PostAsync("/Orders", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));
        
        //Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var location = response.Headers.Location.ToString();
        var getResponse = await httpClient.GetAsync(location);
        var content = await getResponse.Content.ReadAsStringAsync();
        var getOrderResponse = JsonSerializer.Deserialize<GetOrdersResponse>(content, _jsonSerializerOptions);
        Assert.Single(getOrderResponse!.Items);

        var order = getOrderResponse.Items.First();
        Assert.NotNull(order);
        Assert.Equal( 70 , order.TotalPrice);
    }
    
    
    
    
    
}