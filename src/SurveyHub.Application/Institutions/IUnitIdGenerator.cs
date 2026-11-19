namespace SurveyHub.Application.Institutions;

public interface IUnitIdGenerator
{
    Task<int> NextAsync(CancellationToken ct);
}
