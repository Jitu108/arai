namespace PatientSvc.Application.Common.Dto;

public sealed record PatientDto(long PatientId, string MRN, string FirstName, string LastName, DateTime DOB, string Sex);