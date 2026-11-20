using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using SurveyHub.Application.Institutions;
using SurveyHub.Domain.Institutions;

namespace SurveyHub.IntegrationTests;

public class InstitutionEndpointsTests(SurveyHubApiFactory factory) : IClassFixture<SurveyHubApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Post_creates_institution_and_get_returns_it()
    {
        var post = await _client.PostAsJsonAsync("/api/institutions", new { name = "Test University", state = "VA" });

        Assert.Equal(HttpStatusCode.Created, post.StatusCode);
        Assert.NotNull(post.Headers.Location);

        var created = await post.Content.ReadFromJsonAsync<InstitutionDto>();
        Assert.NotNull(created);
        Assert.True(UnitIdCheckDigit.IsValid(created.UnitId));
        Assert.Equal("Test University", created.Name);

        var get = await _client.GetAsync(post.Headers.Location);

        Assert.Equal(HttpStatusCode.OK, get.StatusCode);
        var fetched = await get.Content.ReadFromJsonAsync<InstitutionDto>();
        Assert.Equal(created, fetched);
    }

    [Fact]
    public async Task Post_with_invalid_body_returns_400_with_field_errors()
    {
        var response = await _client.PostAsJsonAsync("/api/institutions", new { name = "", state = "md" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.Contains("Name", problem.Errors.Keys);
        Assert.Contains("State", problem.Errors.Keys);
    }

    [Fact]
    public async Task Get_unknown_unit_id_returns_404()
    {
        var unknown = UnitIdCheckDigit.Append(99998);   // 형식은 맞지만 없는 번호
        var response = await _client.GetAsync($"/api/institutions/{unknown}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_with_invalid_page_returns_400()
    {
        var response = await _client.GetAsync("/api/institutions?page=0");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Health_returns_200()
    {
        var response = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

}