using FluentValidation;
using Speedex.Api.Features.Orders.Requests;
using Speedex.Api.Features.Orders.Validators;
using Speedex.Api.Features.Parcels.Requests;
using Speedex.Api.Features.Parcels.Validators;
using Speedex.Api.Features.Products.Requests;
using Speedex.Api.Features.Products.Validators;
using Speedex.Api.Features.Returns.Requests;
using Speedex.Api.Features.Returns.Validators;
using Speedex.Data;
using Speedex.Data.Generators;
using Speedex.Domain.Orders;
using Speedex.Domain.Parcels;
using Speedex.Domain.Products;
using Speedex.Domain.Returns;

namespace Speedex.Api.Bootstrap;

public static class ApiBootstrap
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services
            .RegisterValidators()
            .RegisterDataGenerators(configuration);

        return services;
    }

    private static IServiceCollection RegisterDataGenerators(this IServiceCollection services, ConfigurationManager configuration)
    {
        services
            .AddSingleton<IDataGenerator, DataGenerator>()
            .AddSingleton<IDataGenerator<OrderId, Order>, OrdersGenerator>()
            .AddSingleton<IDataGenerator<ParcelId, Parcel>, ParcelsGenerator>()
            .AddSingleton<IDataGenerator<ReturnId, Return>, ReturnsGenerator>()
            .AddSingleton<IDataGenerator<ProductId, Product>, ProductsGenerator>();

        services
            .Configure<GenerateOptions>(configuration.GetSection(GenerateOptions.SectionName));

        return services;
    }

    private static IServiceCollection RegisterValidators(this IServiceCollection services)
    {
        services
            .AddSingleton<IValidator<CreateOrderBodyRequest>, CreateOrderValidator>()
            .AddSingleton<IValidator<CreateOrderBodyRequest.RecipientBodyRequest>, RecipientValidator>()
            .AddSingleton<IValidator<CreateOrderBodyRequest.ProductBodyRequest>, ProductValidator>()
            .AddSingleton<IValidator<GetOrdersQueryParams>, GetOrdersValidator>();

        services
            .AddSingleton<IValidator<CreateParcelBodyRequest>, CreateParcelValidator>()
            .AddSingleton<IValidator<CreateParcelBodyRequest.ParcelProductCreateParcelBodyRequest>, ParcelProductCreateParcelBodyRequestValidator>()
            .AddSingleton<IValidator<GetParcelsQueryParams>, GetParcelsValidator>();

        services
            .AddSingleton<IValidator<CreateReturnBodyRequest>, CreateReturnValidator>()
            .AddSingleton<IValidator<CreateReturnBodyRequest.CreateReturnBodyRequestReturnProduct>, CreateReturnBodyRequestReturnProductValidator>()
            .AddSingleton<IValidator<GetReturnsQueryParams>, GetReturnsValidator>();

        services
            .AddSingleton<IValidator<CreateProductBodyRequest>, CreateProductValidator>()
            .AddSingleton<IValidator<CreateProductBodyRequest.DimensionsGetProductBodyRequest>, DimensionsGetProductBodyRequestValidator>()
            .AddSingleton<IValidator<CreateProductBodyRequest.PriceGetProductBodyRequest>, PriceGetProductBodyRequestValidator>()
            .AddSingleton<IValidator<CreateProductBodyRequest.WeightGetProductBodyRequest>, WeightGetProductBodyRequestValidator>()
            .AddSingleton<IValidator<GetProductsQueryParams>, GetProductsValidator>();


        return services;
    }
}