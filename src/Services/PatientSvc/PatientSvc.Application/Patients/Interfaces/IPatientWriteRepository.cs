using PatientSvc.Application.Common.Dto;

namespace PatientSvc.Application.Patients.Interfaces;

public interface IPatientWriteRepository
{
    Task<PatientDto> CreateAsync(string mrn, string first, string last, DateTime dob, string sex, CancellationToken ct = default);
}