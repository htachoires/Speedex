using Speedex.Api.Bootstrap;
using Speedex.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type =>  type.FullName.Replace("+", "."));
});
builder.Services.AddControllers();
builder.Services
    .RegisterDomainServices()
    .RegisterApiServices()
    .RegisterInfrastructureServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Services.GetRequiredService<IDataGenerator>().GenerateData();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();