using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Steeltoe.Discovery.Consul;

namespace Ocelot.ApiGateway.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration
     .SetBasePath(builder.Environment.ContentRootPath)
     .AddOcelot();
            builder.AddServiceDefaults(); 
            builder.Services.AddConsulDiscoveryClient();
            // Ocelot ve Consul'u ekle
            builder.Services.AddOcelot(builder.Configuration)
                .AddConsul();

            builder.Services.AddAuthorization();
            if (builder.Environment.IsDevelopment())
            {
                builder.Logging.AddConsole();
            }
            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Ocelot middleware'i ekle
            await app.UseOcelot();
            app.MapDefaultEndpoints();
            await app.RunAsync();
        }
    }
}
