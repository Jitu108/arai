using GapEngineSvc.Domain.Gaps;
using MediatR;

namespace GapEngineSvc.Application.Queries.GetGapsByPatient;
public sealed record GetGapsByPatientQuery(long PatientId) : IRequest<IReadOnlyList<GapCandidate>>;
