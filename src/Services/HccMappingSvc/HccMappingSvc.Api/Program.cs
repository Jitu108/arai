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
