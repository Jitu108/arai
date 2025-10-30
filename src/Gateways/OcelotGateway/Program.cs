// Program.cs
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Load config files (order matters)
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Register Ocelot
builder.Services.AddOcelot(builder.Configuration);

// (Optional) minimal health endpoint
builder.Services.AddRouting();

var app = builder.Build();

// Optional: quick health check
app.MapGet("/", () => Results.Ok(new { service = "OcelotGateway", status = "OK" }));

// IMPORTANT: UseOcelot returns a Task -> must be awaited BEFORE Run/RunAsync
await app.UseOcelot();

// Use RunAsync to keep the await chain clean
await app.RunAsync();
