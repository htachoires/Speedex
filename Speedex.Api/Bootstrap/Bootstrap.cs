using FluentValidation;
using Speedex.Api.Features.Orders.Requests;
using Speedex.Api.Features.Orders.Validators;
using Speedex.Api.Features.Products.Requests;
using Speedex.Api.Features.Products.Validators;
using Speedex.Data;
using Speedex.Data.Generators;
using Speedex.Domain.Commons;
using Speedex.Domain.Orders;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.UseCases.CreateOrder;
using Speedex.Domain.Orders.UseCases.GetOrders;
using Speedex.Domain.Parcels;
using Speedex.Domain.Parcels.Repositories;
using Speedex.Domain.Products;
using Speedex.Domain.Products.Repositories;
using Speedex.Domain.Products.UseCases.CreateProduct;
using Speedex.Domain.Products.UseCases.GetProducts;
using Speedex.Domain.Returns;
using Speedex.Domain.Returns.Repositories;
using Speedex.Infrastructure;

namespace Speedex.Api.Bootstrap;

public static class Bootstrap
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services
            .RegisterValidators()
            .RegisterRepositories()
            .RegisterCommandHandlers()
            .RegisterQueryHandlers()
            .RegisterDataGenerators();

        return services;
    }

    private static IServiceCollection RegisterCommandHandlers(this IServiceCollection services)
    {
        services
            .AddScoped<ICommandHandler<CreateOrderCommand, CreateOrderResult>, CreateOrderCommandHandler>()
            .AddScoped<ICommandHandler<CreateProductCommand, CreateProductResult>, CreateProductCommandHandler>();

        return services;
    }

    private static IServiceCollection RegisterQueryHandlers(this IServiceCollection services)
    {
        services
            .AddScoped<IQueryHandler<GetOrdersQuery, GetOrdersQueryResult>, GetOrdersQueryHandler>()
            .AddScoped<IQueryHandler<GetProductsQuery, GetProductsQueryResult>, GetProductsQueryHandler>();

        return services;
    }

    private static IServiceCollection RegisterDataGenerators(this IServiceCollection services)
    {
        services
            .AddSingleton<IDataGenerator, DataGenerator>()
            .AddSingleton<IDataGenerator<OrderId, Order>, OrdersGenerator>()
            .AddSingleton<IDataGenerator<ParcelId, Parcel>, ParcelsGenerator>()
            .AddSingleton<IDataGenerator<ReturnId, Return>, ReturnsGenerator>()
            .AddSingleton<IDataGenerator<ProductId, Product>, ProductsGenerator>();

        return services;
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services
            .AddSingleton<IOrderRepository, InMemoryOrderRepository>()
            .AddSingleton<IParcelRepository, InMemoryParcelRepository>()
            .AddSingleton<IReturnRepository, InMemoryReturnRepository>()
            .AddSingleton<IProductRepository, InMemoryProductRepository>();

        return services;
    }

    private static IServiceCollection RegisterValidators(this IServiceCollection services)
    {
        services
            .AddSingleton<IValidator<CreateOrderBodyRequest>, CreateOrderValidator>()
            .AddSingleton<IValidator<CreateOrderBodyRequest.RecipientBodyRequest>, RecipientValidator>()
            .AddSingleton<IValidator<CreateOrderBodyRequest.ProductBodyRequest>, ProductValidator>()
            .AddSingleton<IValidator<GetOrdersQueryParams>, GetOrdersValidator>()
            .AddSingleton<IValidator<CreateProductBodyRequest>, CreateProductValidator>()
            .AddSingleton<IValidator<CreateProductBodyRequest.DimensionsGetProductBodyRequest>, DimensionsGetProductBodyRequestValidator>()
            .AddSingleton<IValidator<CreateProductBodyRequest.PriceGetProductBodyRequest>, PriceGetProductBodyRequestValidator>()
            .AddSingleton<IValidator<CreateProductBodyRequest.WeightGetProductBodyRequest>, WeightGetProductBodyRequestValidator>()
            .AddSingleton<IValidator<GetProductsQueryParams>, GetProductsValidator>();

        return services;
    }
}