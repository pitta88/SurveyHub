using Microsoft.AspNetCore.Http.HttpResults;
using SurveyHub.Application.Common;

namespace SurveyHub.Api.Extensions;

public static class ErrorExtensions
{
    public static ProblemHttpResult ToProblem(this Error error) =>
        TypedResults.Problem(
            title: error.Code,
            detail: error.Description,
            statusCode: error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            }
        );
}