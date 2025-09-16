
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Payment.Infrastructure;
using Payment.Infrastructure.Data;
using System.Reflection;

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
            //builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment API", Version = "v1" });
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://localhost:44310/connect/authorize"),
                            TokenUrl = new Uri("https://localhost:44310/connect/token"),
                            Scopes = new Dictionary<string, string>
                                {
                                    { "payment", "Payment API Full Access" },
                                }
                        }
                    }
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

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
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("v1/swagger.json", "Payment API V1");
                    options.OAuthClientId("payment-api-swaggerui-client");
                    //options.OAuthClientSecret("secret");
                    options.OAuthAppName("saga-orchestration");
                    options.OAuthScopeSeparator(" ");
                    options.OAuthScopes("payment");
                    options.OAuthUsePkce();
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
