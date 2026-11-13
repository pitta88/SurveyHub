
using Microsoft.EntityFrameworkCore;
using SurveyHub.Domain.Institutions;

namespace SurveyHub.Infrastructure.Persistence;

internal static class SampleData
{
    public static void Seed(DbContext context, bool _)
    {
        var institutions = context.Set<Institution>();

        if (institutions.Any()) return;

        string[] states = ["MD", "VA", "DC", "PA", "DE"];
        institutions.AddRange(Enumerable.Range(1, 30).Select(i => new Institution(Institution.UnitIdMin + i, $"Institution {i}", states[(i - 1) % states.Length])));

        context.SaveChanges();
    }
}
