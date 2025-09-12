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
using Payment.Application;
using Payment.Application.Interfaces;
using Payment.Infrastructure.Consumers;
using Payment.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure
{
    public static class ConfigureDependencyInjection
    {
        public class UserStore : UserStore<User, Role, PaymentDbContext, Guid, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>
        {
            public UserStore(PaymentDbContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
            {
            }
        }
        public class AppRoleStore : RoleStore<Role, PaymentDbContext, Guid, UserRole, RoleClaim>
        {
            public AppRoleStore(PaymentDbContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
            {
            }
        }
        public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
        {
            builder.Services.AddBuildingBlocksInfrastructure();

            builder.Services.AddDbContext<PaymentDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("PaymentDbConnection"));
            });
            builder.Services.AddIdentityCore<User>().AddRoles<Role>().AddUserStore<UserStore>().AddRoleStore<AppRoleStore>();
            var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<CompletePaymentMessageConsumer>();
                x.AddEntityFrameworkOutbox<PaymentDbContext>(o =>
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
                    cfg.ReceiveEndpoint(EventBusConstants.Queues.CompletePaymentMessageQueueName, e =>
                    {
                        e.ConfigureConsumer<CompletePaymentMessageConsumer>(context);
                    });
                });
                x.AddConfigureEndpointsCallback((context, name, cfg) =>
                {
                    cfg.UseEntityFrameworkOutbox<PaymentDbContext>(context);
                });
            });
            builder.Services.AddScoped<IMassTransitService, MassTransitService>();
            builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<PaymentDbContext>());
            builder.Services.AddScoped<PaymentDbContextInitializer>();
            builder.Services.AddApplication();
            return  builder;
        }
    }
}
