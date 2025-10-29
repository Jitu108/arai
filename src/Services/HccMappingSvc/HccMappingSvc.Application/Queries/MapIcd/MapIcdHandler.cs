using MediatR;
using HccMappingSvc.Application.Interfaces;

namespace HccMappingSvc.Application.Queries.MapIcd;
public sealed class MapIcdHandler : IRequestHandler<MapIcdQuery, string?>
{
    private readonly IHccMapProvider _provider;
    public MapIcdHandler(IHccMapProvider provider) => _provider = provider;
    public Task<string?> Handle(MapIcdQuery request, CancellationToken ct)
        => _provider.MapIcdToHccAsync(request.Icd, ct);
}
