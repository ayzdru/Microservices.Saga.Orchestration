
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.OpenApi.Models;
using Product.API.Services;
using Product.API.Transformers;
using Product.Application.Commands;
using Product.Application.Interfaces;
using Product.Application.Queries;
using Product.Infrastructure.Data;
using Product.Infrastructure.IoC;
using Product.Web.IoC;

namespace Product.API
{
    public class Program
    {
        const string migrateArg = "/migrate";
        public static async Task Main(string[] args)
        {
            //MigrationService
            if (args.Contains(migrateArg))
            {
                var builder = Host.CreateApplicationBuilder(args);

                builder.AddServiceDefaults();
                builder.Services.AddDbContext<ProductDbContext>((sp, options) =>
                {
                    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());                    
                    options.UseNpgsql(builder.Configuration.GetConnectionString("ProductDbConnection"));
                });
                builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ProductDbContext>());
                builder.Services.AddScoped<ProductDbContextInitializer>();
                builder.Services.AddHostedService<MigrationService>();

                builder.Services.AddOpenTelemetry()
                    .WithTracing(tracing => tracing.AddSource(MigrationService.ActivitySourceName));

                
                var host = builder.Build();
                host.Run();
            }
            else
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
                builder.AddServiceDefaults();
                builder.AddInfrastructure().AddWeb();
                // Add services to the container.
                builder.Services.AddAuthorization();

                // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
                builder.Services.AddOpenApi(options =>
                {
                    options.AddDocumentTransformer((document, context, cancellationToken) =>
                    {
                        document.Info.Contact = new OpenApiContact
                        {
                            Name = "Ayaz DURU",
                            Email = "mail@ayazduru.com.tr"
                        };
                        return Task.CompletedTask;
                    });
                    options.AddDocumentTransformer<OAuth2DocumentTransformer>();
                });

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
                app.MapGet("/api/products", async (IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetProductsQuery());
                    return Results.Ok(result);
                })
    .RequireAuthorization();

                app.MapGet("/api/products/{id}", async (IMediator mediator, Guid id) =>
                {
                    var result = await mediator.Send(new GetProductByIdQuery(id));
                    return result is not null ? Results.Ok(result) : Results.NotFound();
                })
                .RequireAuthorization();

                app.MapPost("/api/products", async (IMediator mediator, CreateProductCommand command) =>
                {
                    var result = await mediator.Send(command);
                    return result is not null ? Results.Created($"/api/products/{result}", result) : Results.NotFound();
                })
                .RequireAuthorization();

                app.MapPut("/api/products/{id}", async (IMediator mediator, UpdateProductCommand command) =>
                {
                    var result = await mediator.Send(command);
                    return result == true ? Results.Ok(result) : Results.NotFound();
                })
                .RequireAuthorization();

                app.MapDelete("/api/products/{id}", async (IMediator mediator, Guid id) =>
                {
                    var result = await mediator.Send(new DeleteProductCommand(id));
                    return result == true ? Results.Ok() : Results.NotFound();
                })
                .RequireAuthorization();
                app.Run();
            }


        }
    }
}
