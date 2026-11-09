
namespace SurveyHub.Application.Institutions;

public sealed record InstitutionDto(
    int Id,
    string UnitId,
    string Name,
    string State
);