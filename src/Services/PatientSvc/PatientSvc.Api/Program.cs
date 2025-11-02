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

// ---------------------------
// Infrastructure & Services
// ---------------------------

// SQL Server connection
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

// Date/Time abstration
builder.Services.AddSingleton<IDateTime, SystemDateTime>();

// Repository implementations
builder.Services.AddScoped<IPatientReadRepository, PatientRepository>();
builder.Services.AddScoped<IPatientWriteRepository, PatientRepository>();

// MediatR (Application assembly)
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        Assembly.Load("PatientSvc.Application")
    );
});

// Diagnostics
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Health checks
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("SqlServer")!);

// Build the app
var app = builder.Build();

// ---------------------------
// Middleware & Swagger
// ---------------------------

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// ---------------------------
// DB Bootstrap with Retry
// ---------------------------

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    const int maxRetries = 30;
    for (int i = 1; i <= maxRetries; i++)
    {
        try
        {
            db.Database.EnsureCreated();  // Phase-1 only; migrations in later phase
            Console.WriteLine("[Startup] SQL connection succeeded.");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Startup] SQL not ready (attempt {i}/{maxRetries}): {ex.Message}");
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}

// ---------------------------
// Endpoints
// ---------------------------

// Health check
app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "patientsvc" }));

// Get single patient
app.MapGet("/api/patients/{id:long}", async (long id, IMediator mediator) =>
{
    var dto = await mediator.Send(new GetPatientByIdQuery(id));
    return dto is null ? Results.NotFound() : Results.Ok(dto);
});

// Get list of patients
app.MapGet("/api/patients", async (int? take, IMediator mediator) =>
{
    var dtos = await mediator.Send(new GetPatientsQuery(take ?? 50));
    return Results.Ok(dtos);
});

// Create patient
app.MapPost("/api/patients", async (PatientCreateRequest req, IMediator mediator) =>
{
    var dto = await mediator.Send(new CreatePatientCommand(req.MRN, req.FirstName, req.LastName, req.DOB, req.Sex));
    return Results.Created($"/api/patients/{dto.PatientId}", dto);
});

// ---------------------------
// Run the app
// ---------------------------
app.Run();

// ---------------------------
// Request DTO
// ---------------------------
public sealed record PatientCreateRequest(string MRN, string FirstName, string LastName, DateTime DOB, string Sex);

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// var app = builder.Build();

// Console.WriteLine("Application Started by Jitendra Kumar Gupta.");

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// //app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "hccmappingsvc" }));
// app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "patientsvc" }));

// var summaries = new[]
// {
//     //"Jitendra", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//     "Jitendra", "Jitendra 1", "Jitendra 2", "Jitendra 3", "Jitendra 4", "Jitendra 5", "Jitendra 6", "Hot", "Jitendra 7", "Jitendra 8"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast = Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     Console.WriteLine("Weather forecast requested By Jitendra");
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

// app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
