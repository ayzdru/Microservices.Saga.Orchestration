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
using Orchestration.Web.IoC;
using Serilog;
using Serilog.Core;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
.MinimumLevel.Information()
.MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
.Enrich.FromLogContext()
.WriteTo.Console()
.CreateLogger();

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



builder.AddInfrastructure().AddWeb();

using IHost host = builder.Build();
using (var scope = host.Services.CreateScope())
{
    var identityInitializer = scope.ServiceProvider.GetRequiredService<OrchestrationDbContextInitializer>();
    await identityInitializer.InitialiseAsync();
    await identityInitializer.SeedAsync();

    var sagaInitializer = scope.ServiceProvider.GetRequiredService<OrchestrationSagaDbContextInitializer>();
    await sagaInitializer.InitialiseAsync();
    await sagaInitializer.SeedAsync();
}
await host.RunAsync();
