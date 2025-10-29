namespace HccMappingSvc.Application.Interfaces;

public interface IHccMapProvider
{
    Task<string?> MapIcdToHccAsync(string icd, CancellationToken ct = default);
}
