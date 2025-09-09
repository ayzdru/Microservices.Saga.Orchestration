using BlazorWebAppOidc;
using BlazorWebAppOidc.Client;
using BlazorWebAppOidc.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;


var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

var identitySettings = builder.Configuration
    .GetSection("Authentication:Schemes:IdentityServer")
    .Get<IdentityServerSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})

.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.Authority = identitySettings.Authority;
    options.ClientId = identitySettings.ClientId;
    options.CallbackPath = identitySettings.CallbackPath;
    options.SignedOutCallbackPath = identitySettings.SignedOutCallbackPath;
    options.RemoteSignOutPath = identitySettings.RemoteSignOutPath;
    options.SignedOutRedirectUri = identitySettings.SignedOutRedirectUri;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.SaveTokens = true;
    options.MapInboundClaims = false;
    options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
    options.TokenValidationParameters.RoleClaimType = "roles";
    foreach (var scope in identitySettings.Scope)
    {
        options.Scope.Add(scope);
    }
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
builder.Services.ConfigureCookieOidc(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);

builder.Services.AddAuthorization();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization(options => options.SerializeAllClaims = true);

builder.Services.AddScoped<IProduct, ServerProduct>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<TokenHandler>();

builder.Services.AddHttpClient("ApiGateway",
      client => client.BaseAddress = new Uri(builder.Configuration["ApiGatewayUri"] ??
          throw new Exception("Missing base address!")))
      .AddHttpMessageHandler<TokenHandler>();

var app = builder.Build();
//Product API group
var productsApi = app.MapGroup("/api/products").RequireAuthorization();

productsApi.MapGet("/", async ([FromServices] IProduct product) =>
{
    return await product.GetProductsAsync();
});

productsApi.MapPost("/", async ([FromServices] IProduct product, [FromBody] Product newProduct) =>
{
    return await product.CreateProductAsync(newProduct);
});

productsApi.MapPut("/{id:guid}", async ([FromServices] IProduct product, Guid id, [FromBody] Product updatedProduct) =>
{
    updatedProduct.Id = id;
    return await product.UpdateProductAsync(updatedProduct);
});

productsApi.MapDelete("/{id:guid}", async ([FromServices] IProduct product, Guid id) =>
{
    return await product.DeleteProductAsync(id);
});
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapStaticAssets();
app.UseAntiforgery();

//app.MapGet("/weather-forecast", ([FromServices] IWeatherForecaster WeatherForecaster) =>
//{
//    return WeatherForecaster.GetWeatherForecastAsync();
//}).RequireAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorWebAppOidc.Client._Imports).Assembly);

app.MapGroup("/authentication").MapLoginAndLogout();
app.MapDefaultEndpoints();
app.Run();
