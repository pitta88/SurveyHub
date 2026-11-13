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
        group.MapGet("/{unitId:int}", GetById);

        return app;
    }

    private static async Task<Results<Ok<PagedResult<InstitutionDto>>, ProblemHttpResult>> GetAll(IOptions<PaginationOptions> options,
        int? page, int? pageSize, IInstitutionQueries institutionQueries, CancellationToken ct)
    {
        var opt = options.Value;
        var (pageRequest, error) = PageRequest.Create(page, pageSize, opt);

        if (pageRequest is null)
        {
            return TypedResults.Problem(error, statusCode: StatusCodes.Status400BadRequest);
        }

        var result = await institutionQueries.GetPageAsync(pageRequest, ct);

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<InstitutionDto>, NotFound>> GetById(int unitId, IInstitutionQueries institutionQueries, CancellationToken ct)
    {

        var institution = await institutionQueries.GetByUnitIdAsync(unitId, ct);
        if (institution is not null)
        {
            return TypedResults.Ok(institution);
        }

        return TypedResults.NotFound();
    }
}