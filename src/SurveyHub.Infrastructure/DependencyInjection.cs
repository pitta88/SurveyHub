using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SurveyHub.Application.Institutions;
using SurveyHub.Domain.Institutions;
using SurveyHub.Infrastructure.Persistence;
using SurveyHub.Infrastructure.Queries;

namespace SurveyHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, bool seedSampleData)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);

            if (seedSampleData)
            {
                options.UseSeeding(SampleData.Seed);
            }
        });

        services.AddSingleton(new SqlConnectionFactory(connectionString));
        services.AddScoped<IInstitutionQueries, InstitutionQueries>();



        return services;
    }
}