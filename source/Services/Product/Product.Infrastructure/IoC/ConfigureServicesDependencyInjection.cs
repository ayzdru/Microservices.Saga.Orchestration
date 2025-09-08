using Product.Application;
using Product.Application.Behaviours;
using Product.Application.Interfaces;
using Product.Application.IoC;
using Product.Core.Entities;
using Product.Core.Interfaces;
using Product.Infrastructure.Data;
using Product.Infrastructure.Data.Interceptors;
using Product.Infrastructure.Interceptors;
using Product.Infrastructure.Services;
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

namespace Product.Infrastructure.IoC
{
    public static class ConfigureServicesDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            services.AddScoped<ISaveChangesInterceptor, EntitySaveChangesInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchNotificationsInterceptor>();

            services.AddTransient<IEmailSender, EmailService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddDbContext<ProductDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(
                    configuration.GetConnectionString("Product"));
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ProductDbContext>());
            services.AddScoped<ProductDbContextInitialiser>();
            services.AddApplication();
            return services;
        }
    }
}
