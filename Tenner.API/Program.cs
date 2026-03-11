using Scalar.AspNetCore;
using StackExchange.Redis;
using Tenner.API.Configuration;
using Tenner.API.Interfaces;
using Tenner.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TenorConfig>(
    builder.Configuration.GetSection("Tenor")
);

builder.Services.AddHttpClient<TenorService>();
builder.Services.AddScoped<ITenorService, TenorService>();

builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect("localhost:6379")
);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
