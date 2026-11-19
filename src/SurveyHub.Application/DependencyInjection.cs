using System.Net;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SurveyHub.Application.Institutions;

namespace SurveyHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateInstitutionHandler>();
        services.AddValidatorsFromAssemblyContaining<CreateInstitutionRequestValidator>();

        return services;
    }
}