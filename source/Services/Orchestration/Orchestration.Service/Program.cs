using BuildingBlocks.MassTransit.Settings;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orchestration.Infrastructure;
using Orchestration.Infrastructure.Data;
using Orchestration.Infrastructure.StateMachines.Order;
using Orchestration.Web;
using Serilog;
using Serilog.Core;
using Serilog.Events;


HostApplicationBuilderSettings settings = new()
{
    Args = args,
    Configuration = new ConfigurationManager(),
    ContentRootPath = Directory.GetCurrentDirectory(),
};

settings.Configuration.AddJsonFile("hostsettings.json", optional: true);
settings.Configuration.AddEnvironmentVariables(prefix: "PREFIX_");
settings.Configuration.AddCommandLine(args);

HostApplicationBuilder builder = Host.CreateApplicationBuilder(settings);

builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    Logger logger = new LoggerConfiguration()
              .ReadFrom.Configuration(builder.Configuration)
              .CreateLogger();

    config.AddSerilog(logger);
});



builder.AddInfrastructure().AddServices();

using IHost host = builder.Build();
using (var scope = host.Services.CreateScope())
{   
    var initializer = scope.ServiceProvider.GetRequiredService<OrchestrationDbContextInitializer>();
    await initializer.InitialiseAsync();
    await initializer.SeedAsync();
}
await host.RunAsync();
