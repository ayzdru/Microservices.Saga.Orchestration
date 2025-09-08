using Product.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Infrastructure.Data;

public class ProductDbContextInitialiser
{
    private readonly ILogger<ProductDbContextInitialiser> _logger;
    private readonly ProductDbContext _context;

    public ProductDbContextInitialiser(ILogger<ProductDbContextInitialiser> logger, ProductDbContext context)
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
        if (!_context.Products.Any())
        {
            var products = new List<Product.Core.Entities.Product>
            {
                new Product.Core.Entities.Product("Laptop", 1500.00m, 10),
                new Product.Core.Entities.Product("Smartphone", 800.00m, 25),
                new Product.Core.Entities.Product("Headphones", 120.00m, 50)
            };

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();
        }
    }
}
