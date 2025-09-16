
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Order.Infrastructure;
using Order.Infrastructure.Data;
using System.Reflection;

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
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Order API", Version = "v1" });
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
                                    { "order", "Order API Full Access" },
                                }
                        }
                    }
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

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
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("v1/swagger.json", "Order API V1");
                    options.OAuthClientId("order-api-swaggerui-client");
                    //options.OAuthClientSecret("secret");
                    options.OAuthAppName("saga-orchestration");
                    options.OAuthScopeSeparator(" ");
                    options.OAuthScopes("order");
                    options.OAuthUsePkce();
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
