using BuildingBlocks.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orchestration.API.Services;


namespace Orchestration.Web.IoC
{
    public static class ConfigureServicesDependencyInjection
    {
        public static HostApplicationBuilder AddWeb(this HostApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            }
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            return builder;
        }
    }
}
