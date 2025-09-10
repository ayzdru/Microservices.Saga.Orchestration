using Payment.Application;
using Payment.Application.Interfaces;
using Payment.Infrastructure.Data;
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

namespace Payment.Infrastructure
{
    public static class ConfigureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            services.AddBuildingBlocksInfrastructure();
            
            services.AddDbContext<PaymentDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(
                    configuration.GetConnectionString("Payment"));
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<PaymentDbContext>());
            services.AddScoped<PaymentDbContextInitializer>();
            services.AddApplication();
            return services;
        }
    }
}
