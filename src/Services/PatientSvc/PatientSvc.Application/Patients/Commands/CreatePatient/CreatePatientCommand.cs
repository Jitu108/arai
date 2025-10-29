using MediatR;
using PatientSvc.Application.Common.Dto;

namespace PatientSvc.Application.Patients.Commands.CreatePatient;

public sealed record CreatePatientCommand(
    string MRN,
    string FirstName,
    string LastName,
    DateTime DOB,
    string Sex
) : IRequest<PatientDto>;