using GapEngineSvc.Domain.Gaps;

namespace GapEngineSvc.Infrastructure.Rules;

public sealed class DiabetesStaticRule : IGapRule
{
    public IEnumerable<GapCandidate> Evaluate(long patientId)
    {
        yield return new GapCandidate(1, patientId, "HCC18", "Diabetes with chronic complications (suspected)", 0.78m);
    }
}

public sealed class ChfStaticRule : IGapRule
{
    public IEnumerable<GapCandidate> Evaluate(long patientId)
    {
        yield return new GapCandidate(2, patientId, "HCC85", "Congestive Heart Failure (suspected)", 0.62m);
    }
}
