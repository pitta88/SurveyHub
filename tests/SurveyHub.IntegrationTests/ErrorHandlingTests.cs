using System.Net;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SurveyHub.Application.Common;
using SurveyHub.Application.Institutions;

namespace SurveyHub.IntegrationTests;

public class ErrorHandlingTests(SurveyHubApiFactory factory) : IClassFixture<SurveyHubApiFactory>
{
    private const string SecretDetail = "Invalid object name 'dbo.SecretTable'";

    private sealed class ThrowingInstitutionQueries : IInstitutionQueries
    {
        public Task<PagedResult<InstitutionDto>> GetPageAsync(PageRequest request, CancellationToken ct)
            => throw new InvalidOperationException(SecretDetail);

        public Task<InstitutionDto?> GetByUnitIdAsync(int unitId, CancellationToken ct)
            => throw new InvalidOperationException(SecretDetail);
    }

    [Fact]
    public async Task Unhandled_exception_returns_500_without_internal_details()
    {
        // Arrange: IInstitutionQueries를 예외를 던지는 가짜로 교체한 앱
        var client = factory.WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IInstitutionQueries>();
                services.AddScoped<IInstitutionQueries, ThrowingInstitutionQueries>();
            })).CreateClient();

        // Act
        var response = await client.GetAsync("/api/institutions");
        var body = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.DoesNotContain(SecretDetail, body);          // 예외 메시지가 새지 않음
        Assert.DoesNotContain("   at ", body);              // 스택 트레이스가 새지 않음
        Assert.Contains("traceId", body);                   // 대신 추적 번호는 있음
    }

    [Fact]
    public async Task Malformed_json_returns_400()
    {
        var client = factory.CreateClient();
        var content = new StringContent("{ \"name\": ", System.Text.Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/institutions", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}