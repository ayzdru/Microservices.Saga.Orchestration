
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Order.Infrastructure;
using Order.Infrastructure.Data;

namespace Order.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddInfrastructure().AddWeb();
            builder.Services.AddControllers();
            // appsettings.json'dan ayarlarý çekiyoruz
            var jwtSettings = builder.Configuration.GetSection("JwtBearer");

            // JWT Bearer authentication ekle
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = jwtSettings["Authority"];
                    options.Audience = jwtSettings["Audience"];
                    options.RequireHttpsMetadata = bool.Parse(jwtSettings["RequireHttpsMetadata"] ?? "true");
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        // Keycloak tokenlarýnda "aud" (audience) kontrolü için
                        ValidateAudience = true
                    };
                });

            builder.Services.AddAuthorizationBuilder();
            builder.AddServiceDefaults();
            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddScoped<TokenHandler>();

            builder.Services.AddHttpClient("ApiGateway",
                  client => client.BaseAddress = new Uri(builder.Configuration["ApiGatewayUri"] ??
                      throw new Exception("Missing base address!")))
                  .AddHttpMessageHandler<TokenHandler>().ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                  {
                      ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                  });
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<OrderDbContextInitializer>();
                await initializer.InitialiseAsync();
                await initializer.SeedAsync();
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapDefaultEndpoints();
            app.MapControllers();
            app.Run();
        }
    }
}
