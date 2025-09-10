using Order.Application;
using Order.Application.Interfaces;
using Order.Infrastructure.Data;
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
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Core.Interfaces;

namespace Order.Infrastructure
{
    public static class ConfigureServicesDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            services.AddBuildingBlocksInfrastructure();

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
