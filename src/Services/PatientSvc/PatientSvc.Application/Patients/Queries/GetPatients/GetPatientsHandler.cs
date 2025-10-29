using MediatR;
using PatientSvc.Application.Common.Dto;
using PatientSvc.Application.Patients.Interfaces;

namespace PatientSvc.Application.Patients.Queries.GetPatients;

public sealed class GetPatientsHandler : IRequestHandler<GetPatientsQuery, IReadOnlyList<PatientDto>>
{
    private readonly IPatientReadRepository _patientReadRepository;

    public GetPatientsHandler(IPatientReadRepository patientReadRepository)
    {
        _patientReadRepository = patientReadRepository;
    }

    public async Task<IReadOnlyList<PatientDto>> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
    {
        return await _patientReadRepository.GetTopAsync(request.Take, cancellationToken);
    }
}