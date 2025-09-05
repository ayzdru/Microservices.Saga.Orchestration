using System;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Identity.UI.Configuration;
using Identity.UI.Configuration.Constants;
using Identity.UI.Helpers;

namespace Microsoft.AspNetCore.Builder;

public static class AdminUiApplicationBuilderExtensions
{
    /// <summary>
    ///     Adds the IdentityServer4 Admin UI to the pipeline of this application. This method must be called
    ///     between UseRouting() and UseEndpoints().
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseIdentityServer4AdminUi(this IApplicationBuilder app)
    {
        app.UseRoutingDependentMiddleware(app.ApplicationServices.GetRequiredService<TestingConfiguration>());

        return app;
    }

    /// <summary>
    ///     Maps the IdentityServer4 Admin UI to the routes of this application.
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="patternPrefix"></param>
    public static IEndpointConventionBuilder
        MapIdentityServer4AdminUi(this IEndpointRouteBuilder endpoint, string patternPrefix = "/")
        => endpoint.MapAreaControllerRoute(CommonConsts.AdminUIArea, CommonConsts.AdminUIArea,
            patternPrefix + "{controller=Home}/{action=Index}/{id?}");

    /// <summary>
    ///     Maps the IdentityServer4 Admin UI health checks to the routes of this application.
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="pattern"></param>
    /// <param name="configureAction"></param>
    public static IEndpointConventionBuilder MapIdentityServer4AdminUiHealthChecks(this IEndpointRouteBuilder endpoint,
        string pattern = "/health", Action<HealthCheckOptions> configureAction = null)
    {
        var options = new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        };

        configureAction?.Invoke(options);

        return endpoint.MapHealthChecks(pattern, options);
    }
}