using System.Reflection;
using GapEngineSvc.Application.Queries.GetGapsByPatient;
using GapEngineSvc.Domain.Gaps;
using GapEngineSvc.Infrastructure.Rules;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Static rule engine for Phase-1
builder.Services.AddSingleton<IGapRule, DiabetesStaticRule>();
builder.Services.AddSingleton<IGapRule, ChfStaticRule>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("GapEngineSvc.Application")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger(); app.UseSwaggerUI();

app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "gapenginesvc" }));

app.MapGet("/api/gaps/patient/{patientId:long}",
    async (long patientId, IMediator mediator) =>
    {
        var list = await mediator.Send(new GetGapsByPatientQuery(patientId));
        return Results.Ok(list);
    });

app.Run();
