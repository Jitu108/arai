using BuildingBlocks.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using PatientSvc.Application.Common.Dto;
using PatientSvc.Application.Patients.Interfaces;
using PatientSvc.Domain.Patients;
using PatientSvc.Infrastructure.Persistence;
using System;

namespace PatientSvc.Infrastructure.Repositories;

public sealed class PatientRepository : IPatientReadRepository, IPatientWriteRepository
{
    private readonly AppDbContext _db;
    private readonly IDateTime _clock;

    public PatientRepository(AppDbContext db, IDateTime clock)
    {
        _db = db; _clock = clock;
    }

    public async Task<PatientDto?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var p = await _db.Patients.AsNoTracking().FirstOrDefaultAsync(x => x.PatientId == id, ct);
        return p is null ? null : new PatientDto(p.PatientId, p.MRN, p.Name.First, p.Name.Last, p.DOB, p.Sex);
    }

    public async Task<IReadOnlyList<PatientDto>> GetTopAsync(int take, CancellationToken ct = default)
    {
        var list = await _db.Patients.AsNoTracking()
            .OrderBy(x => x.PatientId).Take(take)
            .Select(p => new PatientDto(p.PatientId, p.MRN, p.Name.First, p.Name.Last, p.DOB, p.Sex))
            .ToListAsync(ct);

        return list;
    }

    public async Task<PatientDto> CreateAsync(string mrn, string first, string last, DateTime dob, string sex, CancellationToken ct = default)
    {
        var entity = Patient.Create(mrn, first, last, dob, sex, _clock.UtcNow);
        _db.Patients.Add(entity);
        await _db.SaveChangesAsync(ct);
        return new PatientDto(entity.PatientId, entity.MRN, entity.Name.First, entity.Name.Last, entity.DOB, entity.Sex);
    }
}
