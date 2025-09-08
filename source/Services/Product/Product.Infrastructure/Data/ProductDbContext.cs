using Product.Application.Interfaces;
using Product.Core;
using Product.Core.Entities;
using Product.Infrastructure.Extensions;
using Product.Infrastructure.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Product.Infrastructure.Data
{
    public class ProductDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Product.Core.Entities.Product> Products { get; set; }
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
             : base(options)
        {

        }      

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }     

    }
}
