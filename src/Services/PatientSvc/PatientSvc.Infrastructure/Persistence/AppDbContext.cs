using Microsoft.EntityFrameworkCore;
using PatientSvc.Domain.Patients;

namespace PatientSvc.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Patient> Patients => Set<Patient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId);
            entity.HasIndex(e => e.MRN).IsUnique();
            entity.Property(e => e.Sex).HasMaxLength(1);
            entity.Property(e => e.DOB).HasColumnType("date");
            entity.Property(e => e.CreatedAtUtc).HasColumnType("datetime2");

            entity.OwnsOne(x => x.Name, n =>
            {
                n.Property(p => p.First).HasColumnName("FirstName").HasMaxLength(80);
                n.Property(p => p.Last).HasColumnName("LastName").HasMaxLength(80);
            });
            entity.ToTable("Patient", "dbo");
        });
        
        base.OnModelCreating(modelBuilder);
    }
}