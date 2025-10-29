using MediatR;
using PatientSvc.Application.Common.Dto;

namespace PatientSvc.Application.Patients.Queries.GetPatients;

public sealed record GetPatientsQuery(int Take = 50) : IRequest<IReadOnlyList<PatientDto>>;