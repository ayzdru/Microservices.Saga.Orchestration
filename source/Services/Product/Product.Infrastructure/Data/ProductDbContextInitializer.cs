using BuildingBlocks.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Product.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Product.Infrastructure.Data;

public class ProductDbContextInitializer
{
    private readonly ILogger<ProductDbContextInitializer> _logger;
    private readonly ProductDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public ProductDbContextInitializer(ILogger<ProductDbContextInitializer> logger, ProductDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync(CancellationToken cancellationToken)
    {
        try
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await _context.Database.MigrateAsync(cancellationToken);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        try
        {
            await TrySeedAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync(CancellationToken cancellationToken)
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            if (!_context.Products.Any())
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
                var products = new List<Product.Core.Entities.Product>
            {
                new Product.Core.Entities.Product("Laptop", 1500.00m, 10),
                new Product.Core.Entities.Product("Smartphone", 800.00m, 25),
                new Product.Core.Entities.Product("Headphones", 120.00m, 50)
            };

                _context.Products.AddRange(products);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
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
                    await _userManager.AddClaimAsync(identityUser, new Claim("name","admin"));
                    await _userManager.AddToRoleAsync(identityUser, roleName);
                }
            }
        });
    }
}
