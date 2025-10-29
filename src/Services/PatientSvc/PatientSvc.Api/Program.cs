using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PatientSvc.Application.Patients.Commands.CreatePatient;
using PatientSvc.Application.Patients.Interfaces;
using PatientSvc.Application.Patients.Queries.GetPatientById;
using PatientSvc.Application.Patients.Queries.GetPatients;
using PatientSvc.Infrastructure.Persistence;
using PatientSvc.Infrastructure.Repositories;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Infrastructure.Time;

var builder = WebApplication.CreateBuilder(args);

// Infra
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// Clock
builder.Services.AddSingleton<IDateTime, SystemDateTime>();

// Repos
builder.Services.AddScoped<IPatientReadRepository, PatientRepository>();
builder.Services.AddScoped<IPatientWriteRepository, PatientRepository>();

// MediatR (Application assembly)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("PatientSvc.Application")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("SqlServer")!);

var app = builder.Build();
app.UseSwagger(); app.UseSwaggerUI();

// Phase-1 convenience – ensure schema exists; Phase-2 will move to migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Endpoints
app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "patientsvc" }));

app.MapGet("/api/patients/{id:long}",
    async (long id, IMediator mediator) =>
    {
        var dto = await mediator.Send(new GetPatientByIdQuery(id));
        return dto is null ? Results.NotFound() : Results.Ok(dto);
    });

app.MapGet("/api/patients",
    async (int take, IMediator mediator) =>
    {
        var dtos = await mediator.Send(new GetPatientsQuery(take <= 0 ? 50 : take));
        return Results.Ok(dtos);
    });

app.MapPost("/api/patients",
    async (PatientCreateRequest req, IMediator mediator) =>
    {
        var dto = await mediator.Send(new CreatePatientCommand(req.MRN, req.FirstName, req.LastName, req.DOB, req.Sex));
        return Results.Created($"/api/patients/{dto.PatientId}", dto);
    });

app.Run();

public sealed record PatientCreateRequest(string MRN, string FirstName, string LastName, DateTime DOB, string Sex);
