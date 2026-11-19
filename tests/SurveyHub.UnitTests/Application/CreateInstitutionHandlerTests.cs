using Newtonsoft.Json.Serialization;
using SurveyHub.Application.Institutions;
using SurveyHub.Domain.Institutions;

namespace SurveyHub.UnitTests.Application;

public class CreateInstitutionHandlerTests
{
    private sealed class FakeUnitIdGenerator(int unitId) : IUnitIdGenerator
    {
        public Task<int> NextAsync(CancellationToken ct) => Task.FromResult(unitId);
    }

    private sealed class FakeInstitutionRepository : IInstitutionRepository
    {
        public List<Institution> Added { get; } = [];
        public Task AddAsync(Institution institution, CancellationToken ct)
        {
            Added.Add(institution);
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task Creates_institution_with_generated_unit_id()
    {
        var unitId = UnitIdCheckDigit.Append(60001);
        var repository = new FakeInstitutionRepository();
        var handler = new CreateInstitutionHandler(new FakeUnitIdGenerator(unitId), repository);

        var result = await handler.HandleAsync(new CreateInstitutionRequest("New University", "MD"), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(unitId, result.Value.UnitId);
        Assert.Single(repository.Added);
        Assert.Equal("New University", repository.Added[0].Name);
    }
}