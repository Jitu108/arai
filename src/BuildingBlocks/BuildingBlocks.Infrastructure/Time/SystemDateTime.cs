using BuildingBlocks.Application.Interfaces;

namespace BuildingBlocks.Infrastructure.Time;

public class SystemDateTime : IDatetime
{
    public DateTime Now => DateTime.Now;
}