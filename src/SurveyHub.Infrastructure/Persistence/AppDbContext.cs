using Microsoft.EntityFrameworkCore;
using SurveyHub.Domain.Institutions;
using SurveyHub.Domain.Surveys;

namespace SurveyHub.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Institution> Institutions => Set<Institution>();
    public DbSet<Survey> Surveys => Set<Survey>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.HasSequence<int>("UnitIdBaseSequence", "dbo")
            .StartsAt(60000)
            .IncrementsBy(1)
            .HasMin(Institution.UnitIdBaseMin)
            .HasMax(Institution.UnitIdBaseMax);
    }
}