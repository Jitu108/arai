using BuildingBlocks.Domain.Abstractions;

namespace PatientSvc.Domain.Patients;

public sealed class PatientName : ValueObject
{
    public string First { get; }
    public string Last { get; }

    public PatientName(string first, string last)
    {
        First = first;
        Last = last;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return First;
        yield return Last;
    }
}