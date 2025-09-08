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
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            services.AddScoped<ISaveChangesInterceptor, EntitySaveChangesInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchNotificationsInterceptor>();

            services.AddTransient<IEmailSender, EmailService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddDbContext<OrchestrationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(
                    configuration.GetConnectionString("Orchestration"));
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<OrchestrationDbContext>());
            services.AddScoped<OrchestrationDbContextInitialiser>();
            services.AddApplication();
            return services;
        }
    }
}
