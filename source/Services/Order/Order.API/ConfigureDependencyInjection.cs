
using Aspire.ServiceDefaults;
using BuildingBlocks.Core.Interfaces;
using BuildingBlocks.Web;
using Order.API.Services;
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
            builder.Services.AddScoped<TokenHandler>();
            builder.Services.AddHttpClient<ApiGatewayService>(httpClient => httpClient.BaseAddress = new Uri(builder.Configuration["ApiGatewayUri"] ?? throw new Exception("Missing base address!"))).AddConsulServiceDiscovery()             
                .AddHttpMessageHandler<TokenHandler>().ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                 {
                     ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                 });

            return builder;
        }
    }
}
