using BuildingBlocks.Core.Entities;
using BuildingBlocks.Core.Interfaces;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.MassTransit.Interfaces;
using BuildingBlocks.MassTransit.Services;
using FluentValidation;
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
using Orchestration.Application;
using Orchestration.Application.Interfaces;
using Orchestration.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration.Infrastructure
{
    public static class ConfigureDependencyInjection
    {
        public static HostApplicationBuilder AddInfrastructure(this HostApplicationBuilder builder)
        {
            builder.Services.AddBuildingBlocksInfrastructure();
           
            builder.Services.AddDbContext<OrchestrationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(builder.Configuration.GetConnectionString("OrchestrationDbConnection"));
            });
            builder.Services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<IdentityDbContext>();


            builder.Services.AddDbContext<OrchestrationSagaDbContext>((sp, options) =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("OrchestrationDbConnection"),options =>
                {
                    options.MinBatchSize(1);
                });
            });
            builder.Services.AddScoped<IMassTransitService, MassTransitService>();
            builder.Services.AddScoped<IApplicationDbContext, OrchestrationDbContext>();
            builder.Services.AddScoped<OrchestrationDbContextInitializer>();
            builder.Services.AddApplication();
            return builder;
        }
    }
}
