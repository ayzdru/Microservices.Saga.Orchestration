using BuildingBlocks.Core.Entities;
using BuildingBlocks.Core.Interfaces;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Infrastructure.Data.Stores;
using BuildingBlocks.MassTransit.Interfaces;
using BuildingBlocks.MassTransit.Services;
using BuildingBlocks.MassTransit.Settings;
using FluentValidation;
using MassTransit;
using MassTransit.Configuration;
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
using Orchestration.Infrastructure.Consumers.Events;
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
              
            builder.Services.AddIdentityCore<User>()
    .AddRoles<Role>()
    .AddUserStore<AppUserStore<OrchestrationDbContext>>()
    .AddRoleStore<AppRoleStore<OrchestrationDbContext>>();

            builder.Services.AddDbContext<OrchestrationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(builder.Configuration.GetConnectionString("OrchestrationDbConnection"), p =>
                {
                    p.MinBatchSize(1);
                });
            });

            var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<OrchestrationUserRegisteredEventConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.Port, rabbitMqSettings.VirtualHost, h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });
                    cfg.ReceiveEndpoint(EventBusConstants.Queues.OrchestrationUserRegisteredEventQueueName, e =>
                    {
                        e.ConfigureConsumer<OrchestrationUserRegisteredEventConsumer>(context);
                    });
                    cfg.ReceiveEndpoint(EventBusConstants.Queues.CreateOrderMessageQueueName, e => { e.ConfigureSaga<OrderState>(context); });
                });
                x.AddEntityFrameworkOutbox<OrchestrationDbContext>(o =>
                {
                    o.UsePostgres();

                    o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
                });

                x.SetKebabCaseEndpointNameFormatter();


                x.AddSagaStateMachine<OrderStateMachine, OrderState, OrderStateDefinition>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<OrchestrationDbContext>();
                    r.UsePostgres();
                    r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                });

                x.AddConfigureEndpointsCallback((context, name, cfg) =>
                {
                    cfg.UseEntityFrameworkOutbox<OrchestrationDbContext>(context);
                });
            });
            builder.Services.AddScoped<IMassTransitService, MassTransitService>();
            builder.Services.AddScoped<IApplicationDbContext, OrchestrationDbContext>();
            builder.Services.AddScoped<OrchestrationDbContextInitializer>();
            builder.Services.AddApplication();
            return builder;
        }
    }
}
