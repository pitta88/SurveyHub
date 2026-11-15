using SurveyHub.Application.Common;
namespace SurveyHub.Application.Common;

public sealed record PageRequest(int Page, int PageSize)
{
    public int Skip => ((Page - 1) * PageSize);
    public static Result<PageRequest> Create(int? page, int? pageSize, PaginationOptions options)
    {
        var p = page ?? 1;
        var size = pageSize ?? options.DefaultPageSize;

        if (p < 1)
        {
            return Error.Validation("Paging.InvalidPage", "Page must be a positive integer.");
        }

        if (size < 1)
        {
            return Error.Validation("Paging.InvalidPageSize", "PageSize must be a positive integer.");
        }

        return new PageRequest(p, Math.Min(size, options.MaxPageSize));
    }
}