using BuildingBlocks.Core.Entities;
using BuildingBlocks.Core.Interfaces;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.MassTransit.Interfaces;
using BuildingBlocks.MassTransit.Services;
using BuildingBlocks.MassTransit.Settings;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orchestration.Application;
using Orchestration.Application.Interfaces;
using Orchestration.Infrastructure.Data;
using Orchestration.Infrastructure.StateMachines.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration.Infrastructure
{
    public static class ConfigureDependencyInjection
    {
        public static HostApplicationBuilder AddInfrastructure(this HostApplicationBuilder builder)
        {
            builder.Services.AddRouting();
            builder.Services.AddAuthorization();
            builder.Services.AddBuildingBlocksInfrastructure();
           
            builder.Services.AddDbContext<OrchestrationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(builder.Configuration.GetConnectionString("OrchestrationDbConnection"));
            });
            builder.Services.AddIdentityCore<User>().AddRoles<Role>().AddEntityFrameworkStores<OrchestrationDbContext>();



            builder.Services.AddDbContext<OrchestrationSagaDbContext>((sp, options) =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("OrchestrationDbConnection"),options =>
                {
                    options.MinBatchSize(1);
                });
            });

            var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();
            builder.Services.AddMassTransit((Action<IBusRegistrationConfigurator>)(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.Port, rabbitMqSettings.VirtualHost, h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });
                    cfg.ReceiveEndpoint(EventBusConstants.Queues.CreateOrderMessageQueueName, e => { e.ConfigureSaga<OrderStateInstance>(context); });
                });
                //EntityFrameworkOutboxConfigurationExtensions.AddEntityFrameworkOutbox<OrchestrationSagaDbContext>(x, (Action<IEntityFrameworkOutboxConfigurator>?)(o =>
                //{
                //    o.UsePostgres();

                //    o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
                //}));

                x.SetKebabCaseEndpointNameFormatter();


                x.AddSagaStateMachine<OrderStateMachine, OrderStateInstance, OrderStateDefinition>()
                    .EntityFrameworkRepository((Action<IEntityFrameworkSagaRepositoryConfigurator<OrderStateInstance>>)(r =>
                    {
                        r.ExistingDbContext<OrchestrationSagaDbContext>();
                        r.UsePostgres();
                        r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                    }));

            }));
            builder.Services.AddScoped<IMassTransitService, MassTransitService>();
            builder.Services.AddScoped<IApplicationDbContext, OrchestrationDbContext>();
            builder.Services.AddScoped<OrchestrationDbContextInitializer>();
            builder.Services.AddScoped<OrchestrationSagaDbContextInitializer>();
            builder.Services.AddApplication();
            return builder;
        }
    }
}
