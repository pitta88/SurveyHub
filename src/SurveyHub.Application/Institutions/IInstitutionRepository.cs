using SurveyHub.Domain.Institutions;

namespace SurveyHub.Application.Institutions;

public interface IInstitutionRepository
{
    Task AddAsync(Institution institution, CancellationToken ct);
}