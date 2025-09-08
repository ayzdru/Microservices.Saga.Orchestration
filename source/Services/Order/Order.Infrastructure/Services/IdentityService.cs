using Order.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Order.Core.Interfaces;

namespace Order.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly IAuthorizationService _authorizationService;

    public IdentityService(IAuthorizationService authorizationService)
    {        
        _authorizationService = authorizationService;
    }

    public async Task<string?> GetUserNameAsync(Guid userId)
    {
        return "";
    }

    public async Task<bool> IsInRoleAsync(Guid userId, string role)
    {

        return false;
    }

    public async Task<bool> AuthorizeAsync(Guid userId, string policyName)
    {
      
        return false;
    }

}
