using BuildingBlocks.MassTransit.Interfaces;
using BuildingBlocks.MassTransit.Services;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.IoC
{

    public static class ConfigureServicesDependencyInjection
    {
        public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ISaveChangesInterceptor, EntitySaveChangesInterceptor>();
            builder.Services.AddScoped<ISaveChangesInterceptor, DispatchNotificationsInterceptor>();

            builder.Services.AddTransient<IEmailSender, EmailService>();
            builder.Services.AddTransient<IIdentityService, IdentityService>();
            builder.Services.AddDbContext<ProductDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(builder.Configuration.GetConnectionString("ProductDbConnection"));
            }); 
            builder.Services.AddScoped<IMassTransitService, MassTransitService>();
            builder.Services.AddScoped<IApplicationDbContext, ProductDbContext>();
            builder.Services.AddScoped<ProductDbContextInitializer>();
            builder.Services.AddApplication();
            return builder;
        }
    }
}
