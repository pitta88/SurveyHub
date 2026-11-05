var builder = WebApplication.CreateBuilder(args);

// 서비스 등록 (Build 전)
builder.Services.AddProblemDetails();       // Required by UseExceptionHandler to write RFC 9457 error responses
builder.Services.AddHealthChecks();        

var app = builder.Build();

app.UseExceptionHandler();          // Outermost middleware: turns unhandled exceptions into 500 ProblemDetails

app.MapHealthChecks("/health");

app.Run();
