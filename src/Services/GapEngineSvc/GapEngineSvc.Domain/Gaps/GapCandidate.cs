namespace GapEngineSvc.Domain.Gaps;

public sealed record GapCandidate(long GapId, long PatientId, string HccCode, string Description, decimal Confidence);
