using MediatR;
using PatientSvc.Application.Common.Dto;

namespace PatientSvc.Application.Patients.Queries.GetPatientById;

public sealed record GetPatientByIdQuery(long PatientId) : IRequest<PatientDto?>;