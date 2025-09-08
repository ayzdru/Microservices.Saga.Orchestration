
using Product.Core.Interfaces;
using Product.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Product.API.Services;


namespace Product.Web.IoC
{
    public static class ConfigureServicesDependencyInjection
    {
        public static IServiceCollection AddWeb(this IServiceCollection services, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                services.AddDatabaseDeveloperPageExceptionFilter();
            }
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }
}
