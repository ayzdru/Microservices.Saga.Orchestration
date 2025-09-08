
using CleanArchitecture.Application.Commands;
using CleanArchitecture.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Product.Application.Commands;
using Product.Infrastructure.Data;
using Product.Infrastructure.IoC;
using Product.Web.IoC;

namespace Product.API
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
            builder.AddServiceDefaults(); 
            builder.Services.AddInfrastructure(builder.Configuration, builder.Environment).AddWeb(builder.Environment);
            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var initialiser = scope.ServiceProvider.GetRequiredService<ProductDbContextInitialiser>();
                await initialiser.InitialiseAsync();
                await initialiser.SeedAsync();
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
