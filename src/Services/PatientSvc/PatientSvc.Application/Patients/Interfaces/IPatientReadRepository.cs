using PatientSvc.Application.Common.Dto;

namespace PatientSvc.Application.Patients.Interfaces;

public interface IPatientReadRepository
{
    Task<PatientDto?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<PatientDto>> GetTopAsync(int take, CancellationToken ct = default);
}