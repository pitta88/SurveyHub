using SurveyHub.Application.Common;
using SurveyHub.Domain.Institutions;

namespace SurveyHub.Application.Institutions;

public sealed class CreateInstitutionHandler(IUnitIdGenerator unitIdGenerator, IInstitutionRepository repository)
{
    public async Task<Result<InstitutionDto>> HandleAsync(CreateInstitutionRequest request, CancellationToken ct)
    {
        var unitId = await unitIdGenerator.NextAsync(ct);
        var institution = new Institution(unitId, request.Name, request.State);
        await repository.AddAsync(institution, ct);

        return Result<InstitutionDto>.Success(new InstitutionDto(institution.UnitId, institution.Name, institution.State));
    }
}