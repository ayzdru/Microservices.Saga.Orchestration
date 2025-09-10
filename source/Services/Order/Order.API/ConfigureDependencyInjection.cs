
using Order.API.Services;
using BuildingBlocks.Core.Interfaces;
using Order.Application.Services;


namespace Order.API
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
            builder.Services.AddHttpClient<ApiGatewayService>(httpClient => httpClient.BaseAddress = new Uri(builder.Configuration["ApiGatewayUri"] ?? throw new Exception("Missing base address!"))).AddServiceDiscovery();

            return builder;
        }
    }
}
