using Speedex.Api.Bootstrap;
using Speedex.Api.Bootstrap.HostedServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type =>  type.FullName.Replace("+", "."));
});
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services
    .RegisterDomainServices()
    .RegisterApiServices()
    .RegisterInfrastructureServices();

builder.Services.AddHostedService<DataGeneratorHostedService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();