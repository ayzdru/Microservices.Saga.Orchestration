using MassTransit;
using Microservices.Saga.Orchestration.Shared.Infrastructure.Data;
using Microservices.Saga.Orchestration.Shared.Interfaces;
using Microservices.Saga.Orchestration.Shared.StateMachines.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Orchestration.Service
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

            var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<OrderStateDbContext>(x =>
        {
            var connectionString = hostContext.Configuration.GetConnectionString("Default");

            x.UseNpgsql(connectionString, options =>
            {
                options.MinBatchSize(1);
            });
        });        

        services.AddScoped<IOrderValidationService, IOrderValidationService>();
        services.AddMassTransit(x =>
        {
            x.AddEntityFrameworkOutbox<OrderStateDbContext>(o =>
            {
                o.UsePostgres();

                o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
            });

            x.SetKebabCaseEndpointNameFormatter();

           
            x.AddSagaStateMachine<OrderStateMachine, OrderState, OrderStateDefinition>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<OrderStateDbContext>();
                    r.UsePostgres();
                });

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
    })
    .UseSerilog()
    .Build();

            await host.RunAsync();
        }
    }
}
