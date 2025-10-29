using MediatR;
using PatientSvc.Application.Common.Dto;
using PatientSvc.Application.Patients.Interfaces;

namespace PatientSvc.Application.Patients.Queries.GetPatientById;

public sealed class GetPatientByIdHandler : IRequestHandler<GetPatientByIdQuery, PatientDto?>
{
    private readonly IPatientReadRepository _patientReadRepository;

    public GetPatientByIdHandler(IPatientReadRepository patientReadRepository)
    {
        _patientReadRepository = patientReadRepository;
    }

    public async Task<PatientDto?> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
    {
        return await _patientReadRepository.GetByIdAsync(request.PatientId, cancellationToken);
    }
}