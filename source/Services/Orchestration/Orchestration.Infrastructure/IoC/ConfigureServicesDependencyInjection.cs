using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orchestration.Application;
using Orchestration.Application.Behaviours;
using Orchestration.Application.Interfaces;
using Orchestration.Application.IoC;
using Orchestration.Core.Entities;
using Orchestration.Core.Interfaces;
using Orchestration.Infrastructure.Data;
using Orchestration.Infrastructure.Data.Interceptors;
using Orchestration.Infrastructure.Interceptors;
using Orchestration.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration.Infrastructure.IoC
{
    public static class ConfigureServicesDependencyInjection
    {
        public static HostApplicationBuilder AddInfrastructure(this HostApplicationBuilder builder)
        {
            builder.Services.AddScoped<ISaveChangesInterceptor, EntitySaveChangesInterceptor>();
            builder.Services.AddScoped<ISaveChangesInterceptor, DispatchNotificationsInterceptor>();

            builder.Services.AddTransient<IEmailSender, EmailService>();
            builder.Services.AddTransient<IIdentityService, IdentityService>();
            builder.Services.AddDbContext<OrchestrationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(builder.Configuration.GetConnectionString("OrchestrationDbConnection"));
            });
            builder.Services.AddScoped<IApplicationDbContext, OrchestrationDbContext>();
            builder.Services.AddScoped<OrchestrationDbContextInitializer>();
            builder.Services.AddApplication();
            return builder;
        }
    }
}
