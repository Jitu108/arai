using MediatR;
using PatientSvc.Application.Common.Dto;
using PatientSvc.Application.Patients.Interfaces;

namespace PatientSvc.Application.Patients.Commands.CreatePatient;

public sealed class CreatePatientHandler : IRequestHandler<CreatePatientCommand, PatientDto>
{
    private readonly IPatientWriteRepository _patientWriteRepository;

    public CreatePatientHandler(IPatientWriteRepository patientWriteRepository)
    {
        _patientWriteRepository = patientWriteRepository;
    }

    public async Task<PatientDto> Handle(CreatePatientCommand request, CancellationToken ct)
    {
        return await _patientWriteRepository.CreateAsync(
            request.MRN,
            request.FirstName,
            request.LastName,
            request.DOB,
            request.Sex,
            ct);
    }
}