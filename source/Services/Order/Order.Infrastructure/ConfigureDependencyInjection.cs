using BuildingBlocks.Core.Entities;
using BuildingBlocks.Core.Interfaces;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.MassTransit.Interfaces;
using BuildingBlocks.MassTransit.Services;
using BuildingBlocks.MassTransit.Settings;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Order.Application;
using Order.Application.Interfaces;
using Order.Infrastructure.Consumers;
using Order.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure
{
    public static class ConfigureDependencyInjection
    {
        public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
        {
            builder.Services.AddBuildingBlocksInfrastructure();

            builder.Services.AddDbContext<OrderDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("OrderDbConnection"));
            });
            builder.Services.AddIdentityCore<User>().AddRoles<Role>().AddEntityFrameworkStores<OrderDbContext>();
            var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderCompletedEventConsumer>();
                x.AddConsumer<OrderFailedEventConsumer>();
                //x.AddEntityFrameworkOutbox<OrderDbContext>(o =>
                //{
                //    o.QueryDelay = TimeSpan.FromSeconds(1);

                //    o.UsePostgres();
                //    o.UseBusOutbox();
                //});

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.Port, rabbitMqSettings.VirtualHost, h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });
                    cfg.AutoStart = true;
                    cfg.ReceiveEndpoint(EventBusConstants.Queues.OrderCompletedEventQueueName, x =>
                    {
                        x.ConfigureConsumer<OrderCompletedEventConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(EventBusConstants.Queues.OrderFailedEventQueueName, x =>
                    {
                        x.ConfigureConsumer<OrderFailedEventConsumer>(context);
                    });
                });
            });
            builder.Services.AddScoped<IMassTransitService, MassTransitService>();
            builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<OrderDbContext>());
            builder.Services.AddScoped<OrderDbContextInitializer>();
            builder.Services.AddApplication();
            return builder;
        }
    }
}
