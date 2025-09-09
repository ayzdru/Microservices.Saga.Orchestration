using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using Ocelot.Provider.Consul;

namespace Ocelot.ApiGateway.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {         
            var builder = WebApplication.CreateBuilder(args);
           

            var jwtSettings = builder.Configuration.GetSection("JwtBearer");
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
            builder.Configuration
     .SetBasePath(builder.Environment.ContentRootPath)
     .AddOcelot();
            builder.AddServiceDefaults();
            // Ocelot ve Consul'u ekle
            builder.Services.AddOcelot(builder.Configuration).AddConsul();

            if (builder.Environment.IsDevelopment())
            {
                builder.Logging.AddConsole();
            }
            var app = builder.Build();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

          
            app.MapDefaultEndpoints();
            app.UseHealthChecks("/health");
            //En son satýra Ocelot middleware'i ekle
            await app.UseOcelot();
            await app.RunAsync();
        }
    }
}
