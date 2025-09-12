using BuildingBlocks.Application.Behaviours;
using BuildingBlocks.Core.Entities;
using BuildingBlocks.Core.Interfaces;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Infrastructure.Data.Interceptors;
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
using Product.Application;
using Product.Application.Interfaces;
using Product.Core.Entities;
using Product.Infrastructure.Consumers.Events;
using Product.Infrastructure.Data;
using Subscription.Infrastructure.Consumers.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure
{

    public static class ConfigureDependencyInjection
    {
        public class UserStore : UserStore<User, Role, ProductDbContext, Guid, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>
        {
            public UserStore(ProductDbContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
            {
            }
        }
        public class AppRoleStore : RoleStore<Role, ProductDbContext, Guid, UserRole, RoleClaim>
        {
            public AppRoleStore(ProductDbContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
            {
            }
        }
        public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
        {
            builder.Services.AddBuildingBlocksInfrastructure();
           
            builder.Services.AddDbContext<ProductDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(builder.Configuration.GetConnectionString("ProductDbConnection"));
            });
            builder.Services.AddIdentityCore<User>().AddRoles<Role>().AddUserStore<UserStore>().AddRoleStore<AppRoleStore>();
            var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderCreatedEventConsumer>();
                x.AddConsumer<StockRollBackMessageConsumer>();

                x.AddEntityFrameworkOutbox<ProductDbContext>(o =>
                {
                    o.QueryDelay = TimeSpan.FromSeconds(1);

                    o.UsePostgres();
                    o.UseBusOutbox();
                });

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.Port, rabbitMqSettings.VirtualHost, h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });
                    cfg.AutoStart = true;
                    cfg.ReceiveEndpoint(EventBusConstants.Queues.OrderCreatedEventQueueName, e =>
                    {
                        e.ConfigureConsumer<OrderCreatedEventConsumer>(context);                        
                    });

                    cfg.ReceiveEndpoint(EventBusConstants.Queues.StockRollBackMessageQueueName, e =>
                    {
                        e.ConfigureConsumer<StockRollBackMessageConsumer>(context);
                    });
                });
                x.AddConfigureEndpointsCallback((context, name, cfg) =>
                {
                    cfg.UseEntityFrameworkOutbox<ProductDbContext>(context);
                });
            });
            builder.Services.AddScoped<IMassTransitService, MassTransitService>();
            builder.Services.AddScoped<IApplicationDbContext, ProductDbContext>();
            builder.Services.AddScoped<ProductDbContextInitializer>();
            builder.Services.AddApplication();
            return builder;
        }
    }
}
