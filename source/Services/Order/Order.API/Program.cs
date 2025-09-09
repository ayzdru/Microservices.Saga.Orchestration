
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace Order.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
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

            var app = builder.Build();

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
