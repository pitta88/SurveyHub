using SurveyHub.Application.Common;
using SurveyHub.Api.Endpoints;
using Scalar.AspNetCore;
using SurveyHub.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<PaginationOptions>()
    .Bind(builder.Configuration.GetSection(PaginationOptions.SectionName))
    .ValidateDataAnnotations()
    .Validate(o => o.DefaultPageSize <= o.MaxPageSize, "DefaultPageSize must be less than or equal to MaxPageSize.")
    .ValidateOnStart(); // Validate options on application startup

builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("SurveyHub")
    ?? throw new InvalidOperationException("Connection string 'SurveyHub' is missing.");

builder.Services.AddInfrastructure(connectionString, seedSampleData: builder.Environment.IsDevelopment());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();          // Outermost middleware: turns unhandled exceptions into 500 ProblemDetails

app.MapInstitutionEndpoints();
app.MapHealthChecks("/health");

app.Run();
