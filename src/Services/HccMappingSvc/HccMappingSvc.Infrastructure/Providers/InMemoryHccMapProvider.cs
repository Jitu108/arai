using HccMappingSvc.Application.Interfaces;

namespace HccMappingSvc.Infrastructure.Providers;

public sealed class InMemoryHccMapProvider : IHccMapProvider
{
    private static readonly Dictionary<string, string> Map = new(StringComparer.OrdinalIgnoreCase)
    {
        ["E11.22"] = "HCC18",
        ["I50.9"] = "HCC85",
        ["J44.9"] = "HCC111"
    };

    public Task<string?> MapIcdToHccAsync(string icd, CancellationToken ct = default)
        => Task.FromResult(Map.TryGetValue(icd, out var hcc) ? hcc : null);
}
