using GapEngineSvc.Domain.Gaps;

namespace GapEngineSvc.Domain.Gaps;
public interface IGapRule
{
    IEnumerable<GapCandidate> Evaluate(long patientId);
}
