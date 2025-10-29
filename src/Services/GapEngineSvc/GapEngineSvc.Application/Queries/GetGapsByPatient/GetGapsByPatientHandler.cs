using GapEngineSvc.Domain.Gaps;
using MediatR;

namespace GapEngineSvc.Application.Queries.GetGapsByPatient;

public sealed class GetGapsByPatientHandler : IRequestHandler<GetGapsByPatientQuery, IReadOnlyList<GapCandidate>>
{
    private readonly IEnumerable<IGapRule> _rules;
    public GetGapsByPatientHandler(IEnumerable<IGapRule> rules) => _rules = rules;

    public Task<IReadOnlyList<GapCandidate>> Handle(GetGapsByPatientQuery request, CancellationToken ct)
    {
        var results = _rules.SelectMany(r => r.Evaluate(request.PatientId)).ToList();
        return Task.FromResult<IReadOnlyList<GapCandidate>>(results);
    }
}
