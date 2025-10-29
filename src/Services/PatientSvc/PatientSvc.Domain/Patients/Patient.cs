using BuildingBlocks.Application.Interfaces;


namespace PatientSvc.Domain.Patients;

public sealed class Patient : IAggregateRoot
{
    public long PatientId { get; private set; }
    public string MRN { get; private set; } = default!;
    public PatientName Name { get; private set; } = default!;
    public DateTime DOB { get; private set; }
    public string Sex { get; private set; } = "U";
    public DateTime CreatedAtUtc { get; private set; }

    private Patient() { }

    private Patient(string mrn, PatientName name, DateTime dob, string sex, DateTime createdAtUtc)
    {
        MRN = mrn;
        Name = name;
        DOB = dob;
        Sex = sex;
        CreatedAtUtc = createdAtUtc;
    }

    public static Patient Create(string mrn, string first, string last, DateTime dob, string sex, DateTime utcnow) =>
    new(mrn, new PatientName(first, last), dob, sex, utcnow);
}