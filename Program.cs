using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SendGrid;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        var sendGridApiKey = Environment.GetEnvironmentVariable("SendGridApiKey") 
            ?? throw new InvalidOperationException("SendGridApiKey environment variable is not configured.");
        
        services.AddSingleton<ISendGridClient>(new SendGridClient(sendGridApiKey));
    })
    .Build();

host.Run();
