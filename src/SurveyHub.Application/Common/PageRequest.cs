namespace SurveyHub.Application.Common;

public sealed record PageRequest(int Page, int PageSize)
{
    public int Skip => ((Page - 1) * PageSize);
    public static (PageRequest? Request, string? Error) Create(int? page, int? pageSize, PaginationOptions options)
    {
        var p = page ?? 1;
        var size = pageSize ?? options.DefaultPageSize;

        if (p < 1)
        {
            return (null, "Page must be a positive integer.");
        }

        if (size < 1)
        {
            return (null, "PageSize must be a positive integer.");
        }

        return (new PageRequest(p, Math.Min(size, options.MaxPageSize)), null);    
    }
}