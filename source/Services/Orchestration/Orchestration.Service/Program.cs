using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orchestration.Core.Entities;
using Orchestration.Core.Interfaces;
using Orchestration.Infrastructure.Data;
using Orchestration.Infrastructure.IoC;
using Orchestration.Infrastructure.Services;
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

builder.Services.AddDbContext<OrchestrationDbContext>(x =>
{
    var connectionString = builder.Configuration.GetConnectionString("OrchestrationDbConnection");

    x.UseNpgsql(connectionString, options =>
    {
        options.MinBatchSize(1);
    });
});

builder.AddInfrastructure().AddWeb();
var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.Port, rabbitMqSettings.VirtualHost, h =>
        {
            h.Username(rabbitMqSettings.Username);
            h.Password(rabbitMqSettings.Password);
        });

        cfg.ConfigureEndpoints(context);
    });
    x.AddEntityFrameworkOutbox<OrchestrationDbContext>(o =>
    {
        o.UsePostgres();

        o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
    });

    x.SetKebabCaseEndpointNameFormatter();


    x.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
        .EntityFrameworkRepository(r =>
        {
            r.ExistingDbContext<OrchestrationDbContext>();
            r.UsePostgres();
        });
    
});
using IHost host = builder.Build();
using (var scope = host.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<OrchestrationDbContextInitializer>();
    await initializer.InitialiseAsync();
    await initializer.SeedAsync();
}
await host.RunAsync();
