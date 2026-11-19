using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using SurveyHub.Api.Extensions;
using SurveyHub.Api.Filters;
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

        group.MapGet("/{unitId:int}", GetById)
            .WithName("GetInstitution");

        group.MapPost("/", Create)
            .AddEndpointFilter<ValidationFilter<CreateInstitutionRequest>>()
            .ProducesValidationProblem();

        return app;
    }

    private static async Task<Results<Ok<PagedResult<InstitutionDto>>, ProblemHttpResult>> GetAll(IOptions<PaginationOptions> options,
        int? page, int? pageSize, IInstitutionQueries institutionQueries, CancellationToken ct)
    {
        var opt = options.Value;
        var pageRequest = PageRequest.Create(page, pageSize, opt);

        if (pageRequest.IsFailure)
        {
            return pageRequest.Error.ToProblem();
        }

        var result = await institutionQueries.GetPageAsync(pageRequest.Value, ct);

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

    private static async Task<Results<CreatedAtRoute<InstitutionDto>, ProblemHttpResult>> Create(CreateInstitutionRequest request, CreateInstitutionHandler handler, CancellationToken ct)
    {
        var result = await handler.HandleAsync(request, ct);
        if (result.IsFailure)
        {
            return result.Error.ToProblem();
        }

        return TypedResults.CreatedAtRoute(result.Value, "GetInstitution", new { unitid = result.Value.UnitId });
    }
}