using BuildingBlocks.Application.Behaviours;
using BuildingBlocks.Core.Interfaces;
using BuildingBlocks.Infrastructure.Data.Interceptors;
using Discount.Infrastructure.Identity;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Order.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Infrastructure
{
    public static class ConfigureDependencyInjection
    {
        public static IServiceCollection AddBuildingBlocksInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ISaveChangesInterceptor, EntitySaveChangesInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchNotificationsInterceptor>();
            services.AddTransient<IEmailSender, EmailService>();
            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }
    }
}
