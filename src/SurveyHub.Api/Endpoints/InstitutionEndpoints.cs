using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using SurveyHub.Application.Common;
using SurveyHub.Application.Institutions;

namespace SurveyHub.Api.Endpoints;

public static class InstitutionEndpoints
{
    public static IEndpointRouteBuilder MapInstitutionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/institutions")
            .WithTags("Institutions");

        group.MapGet("/", GetAll)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        group.MapGet("/{id:int}", GetById);

        return app;
    }

    private static readonly string[] States = ["MD", "VA", "DC", "PA", "DE"];

    private static readonly IReadOnlyDictionary<int, InstitutionDto> Institutions =
        Enumerable.Range(1, 30).ToDictionary(
            i => i,
            i => new InstitutionDto(
                Id: i,
                UnitId: $"{100000 + i}",
                Name: $"Institution {i}",
                State: States[i % States.Length]
                )
            );

    private static Results<Ok<PagedResult<InstitutionDto>>, ProblemHttpResult> GetAll(IOptions<PaginationOptions> options,
        int? page, int? pageSize)
    {
        var opt = options.Value;
        var (pageRequest, error) = PageRequest.Create(page, pageSize, opt);

        if (pageRequest is null)
        {
            return TypedResults.Problem(error, statusCode: StatusCodes.Status400BadRequest);
        }

        var items = Institutions.Values
            .OrderBy(i => i.Id)
            .Skip(pageRequest.Skip).Take(pageRequest.PageSize)
            .ToList();

        return TypedResults.Ok(new PagedResult<InstitutionDto>(items, pageRequest.Page, pageRequest.PageSize, Institutions.Count));
    }

    private static Results<Ok<InstitutionDto>, NotFound> GetById(int id)
    {
        if (Institutions.TryGetValue(id, out var institution))
        {
            return TypedResults.Ok(institution);
        }

        return TypedResults.NotFound();
    }
}