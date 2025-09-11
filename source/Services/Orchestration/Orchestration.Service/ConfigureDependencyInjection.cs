using BuildingBlocks.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orchestration.API.Services;


namespace Orchestration.Web
{
    public static class ConfigureDependencyInjection
    {
        public static HostApplicationBuilder AddServices(this HostApplicationBuilder builder)
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
