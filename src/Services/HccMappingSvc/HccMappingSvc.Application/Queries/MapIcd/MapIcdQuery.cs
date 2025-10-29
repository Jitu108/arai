using MediatR;

namespace HccMappingSvc.Application.Queries.MapIcd;
public sealed record MapIcdQuery(string Icd) : IRequest<string?>;
