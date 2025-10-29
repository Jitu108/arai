using BuildingBlocks.Application.Interfaces;

namespace BuildingBlocks.Infrastructure.Time;

public class SystemDateTime : IDateTime
{
    public DateTime Now => DateTime.Now;

    public DateTime UtcNow => DateTime.UtcNow;

    public DateOnly Today => DateOnly.FromDateTime(DateTime.Now);
}