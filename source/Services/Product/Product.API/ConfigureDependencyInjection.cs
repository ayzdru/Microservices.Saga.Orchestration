
using Product.API.Services;
using BuildingBlocks.Core.Interfaces;


namespace Product.API
{
    public static class ConfigureDependencyInjection
    {
        public static WebApplicationBuilder AddWeb(this WebApplicationBuilder builder)
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
