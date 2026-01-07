using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SendGrid;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

var sendGridApiKey = Environment.GetEnvironmentVariable("SendGridApiKey");
builder.Services.AddSingleton<ISendGridClient>(new SendGridClient(sendGridApiKey));

builder.Build().Run();
