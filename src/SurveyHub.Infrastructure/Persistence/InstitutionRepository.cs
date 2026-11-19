using SurveyHub.Application.Institutions;
using SurveyHub.Domain.Institutions;

namespace SurveyHub.Infrastructure.Persistence;

internal sealed class InstitutionRepository(AppDbContext db) : IInstitutionRepository
{
    public async Task AddAsync(Institution institution, CancellationToken ct)
    {
        db.Institutions.Add(institution);
        await db.SaveChangesAsync(ct);
    }
}