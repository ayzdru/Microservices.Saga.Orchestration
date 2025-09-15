
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Payment.Infrastructure;
using Payment.Infrastructure.Data;

namespace Payment.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddInfrastructure().AddWeb();
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
                        ValidateAudience = true
                    };
                });

            builder.Services.AddAuthorizationBuilder();
            builder.AddServiceDefaults();
            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<PaymentDbContextInitializer>();
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
            app.Run();
        }
    }
}
