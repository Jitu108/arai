using System.Reflection;
using HccMappingSvc.Application.Interfaces;
using HccMappingSvc.Application.Queries.MapIcd;
using HccMappingSvc.Infrastructure.Providers;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHccMapProvider, InMemoryHccMapProvider>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("HccMappingSvc.Application")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger(); app.UseSwaggerUI();

app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "hccmappingsvc" }));

app.MapGet("/api/hcc/map", async (string icd, IMediator mediator) =>
{
    var hcc = await mediator.Send(new MapIcdQuery(icd));
    return Results.Ok(new { icd, hcc });
});

app.Run();

// // Add services to the container.
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

// app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
