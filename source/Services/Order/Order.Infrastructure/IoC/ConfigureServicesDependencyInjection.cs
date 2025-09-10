using Order.Application;
using Order.Application.Behaviours;
using Order.Application.Interfaces;
using Order.Application.IoC;
using Order.Core.Entities;
using Order.Core.Interfaces;
using Order.Infrastructure.Data;
using Order.Infrastructure.Data.Interceptors;
using Order.Infrastructure.Interceptors;
using Order.Infrastructure.Services;
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

namespace Order.Infrastructure.IoC
{
    public static class ConfigureServicesDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            services.AddScoped<ISaveChangesInterceptor, EntitySaveChangesInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchNotificationsInterceptor>();

            services.AddTransient<IEmailSender, EmailService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddDbContext<OrderDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(
                    configuration.GetConnectionString("Order"));
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<OrderDbContext>());
            services.AddScoped<OrderDbContextInitializer>();
            services.AddApplication();
            return services;
        }
    }
}
