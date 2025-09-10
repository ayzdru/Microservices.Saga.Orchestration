using BuildingBlocks.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Data;

public class PaymentDbContextInitializer
{
    private readonly ILogger<PaymentDbContextInitializer> _logger;
    private readonly PaymentDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public PaymentDbContextInitializer(ILogger<PaymentDbContextInitializer> logger, PaymentDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {

            var roleName = "Administrator";
            if (_context.Roles.Any() == false)
            {

                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var role = new Role
                    {
                        Name = roleName
                    };

                    var result = await _roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {

                    }
                }
            }
            if (_context.Users.Any() == false)
            {
                var identityUser = new User
                {
                    Id = Guid.Parse("d78c3a48-d29b-42c1-b4ad-6fe527fb00d2"),
                    UserName = "admin",
                    Email = "admin@email.com",
                    EmailConfirmed = true
                };




                var result = await _userManager.CreateAsync(identityUser);

                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(identityUser, new Claim("name", "admin"));
                    await _userManager.AddToRoleAsync(identityUser, roleName);
                }
            }
        });
    }
}
