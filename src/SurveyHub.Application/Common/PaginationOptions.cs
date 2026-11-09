using System.ComponentModel.DataAnnotations;

namespace SurveyHub.Application.Common;

public sealed class PaginationOptions
{
    public const string SectionName = "Pagination";

    [Range(1, 500)]
    public int DefaultPageSize { get; set; } = 20;

    [Range(1, 500)]
    public int MaxPageSize { get; set; } = 100;

    
}
