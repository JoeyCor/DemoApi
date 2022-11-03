using DemoApi.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddHealthChecks()
    .AddCheck<WorkingHealthCheck>("WorkingHealthCheck")
    .AddCheck<ChaosHealthCheck>("ChaosHealthCheck");

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/hostinfo", () =>
{
    var hostName = Dns.GetHostName();
    var ipAddresses = Dns.GetHostAddresses(hostName).Select(i => i.ToString());
    return new HostInfo(hostName, string.Join(',', ipAddresses));
})
.WithName("GetHostInfo");

app.MapGet("/healthy", () =>
{
    return "this is fine";
}).WithName("healthy");

app.MapGet("/faulty", () =>
{
    throw new Exception("This is not fine");
}).WithName("faulty");

app.MapGet("/chaos", () =>
{
    Random random = new Random();

    if (random.Next(0, 100) <= 80)
    {
        throw new Exception("This is not fine");
    }

    return "This is fine.";
}).WithName("chaos");

app.MapHealthChecks("/healthz");

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal record HostInfo(string HostName, string ipAddresses) { }
