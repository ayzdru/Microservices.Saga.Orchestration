using BuildingBlocks.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Orchestration.Infrastructure.Data;

public class OrchestrationSagaDbContextInitializer
{
    private readonly ILogger<OrchestrationSagaDbContextInitializer> _logger;
    private readonly OrchestrationSagaDbContext _context;

    public OrchestrationSagaDbContextInitializer(ILogger<OrchestrationSagaDbContextInitializer> logger, OrchestrationSagaDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _logger = logger;
        _context = context;
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

           
        });
    }
}
