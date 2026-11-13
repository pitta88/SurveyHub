using SurveyHub.Application.Common;

namespace SurveyHub.Application.Institutions;

public interface IInstitutionQueries
{
    Task<PagedResult<InstitutionDto>> GetPageAsync(PageRequest pageRequest, CancellationToken ct);
    Task<InstitutionDto?> GetByUnitIdAsync(int unitId, CancellationToken ct);
}