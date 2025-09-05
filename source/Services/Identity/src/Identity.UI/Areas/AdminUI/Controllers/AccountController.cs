using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Identity.UI.Configuration.Constants;

namespace Identity.UI.Areas.AdminUI.Controllers;

[Authorize]
[Area(CommonConsts.AdminUIArea)]
public class AccountController : BaseController
{
    public AccountController(ILogger<ConfigurationController> logger) : base(logger)
    {
    }

    public IActionResult AccessDenied() => View();

    public IActionResult Logout() => new SignOutResult(new List<string>
        { AuthenticationConsts.SignInScheme, AuthenticationConsts.OidcAuthenticationScheme });
}