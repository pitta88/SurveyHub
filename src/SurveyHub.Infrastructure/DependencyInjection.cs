using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using SurveyHub.Domain.Institutions;
using SurveyHub.Infrastructure.Persistence;

namespace SurveyHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, bool seedSampleData)
    {
        // services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);

            if (seedSampleData)
            {
                options.UseSeeding((context, _) =>
                {
                   var institutions = context.Set<Institution>();
                   if (institutions.Any()) return;

                   string[] states = ["MD", "VA", "DC", "PA", "DE"];
                   institutions.AddRange(Enumerable.Range(1,30).Select(i=> new Institution(Institution.UnitIdMin + i, $"Institution {i}", states[(i-1)%states.Length])));

                   context.SaveChanges();
                });      
            }
        });

        return services;
    }
}