using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Identity.IntegrationTests.Common;
using Identity.IntegrationTests.Tests.Base;
using Identity.UI.Configuration.Constants;
using Xunit;

namespace Identity.IntegrationTests.Tests;

public class ConfigurationControllerTests : BaseClassFixture
{
    public ConfigurationControllerTests(TestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task ReturnSuccessWithAdminRole()
    {
        SetupAdminClaimsViaHeaders();

        foreach (var route in RoutesConstants.GetConfigureRoutes())
        {
            // Act
            var response = await Client.GetAsync($"/Configuration/{route}");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }


    [Fact]
    public async Task ReturnRedirectWithoutAdminRole()
    {
        //Remove
        Client.DefaultRequestHeaders.Clear();

        foreach (var route in RoutesConstants.GetConfigureRoutes())
        {
            // Act
            var response = await Client.GetAsync($"/Configuration/{route}");

            // Assert           
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);

            //The redirect to login
            response.Headers.Location.ToString().Should().Contain(AuthenticationConsts.AccountLoginPage);
        }
    }
}