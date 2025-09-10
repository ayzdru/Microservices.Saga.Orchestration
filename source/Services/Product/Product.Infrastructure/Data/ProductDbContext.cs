using BuildingBlocks.Core.Entities;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Product.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Product.Infrastructure.Data
{
    public class ProductDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>, IApplicationDbContext
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
            ConfigureIdentity(builder);
            builder.AddInboxStateEntity();
            builder.AddOutboxMessageEntity();
            builder.AddOutboxStateEntity();
        }
        private void ConfigureIdentity(ModelBuilder builder)
        {
            builder.Entity<Role>().ToTable(nameof(Roles));
            builder.Entity<RoleClaim>().ToTable(nameof(RoleClaims));
            builder.Entity<UserRole>().ToTable(nameof(UserRoles));
            builder.Entity<User>().ToTable(nameof(Users));
            builder.Entity<UserLogin>().ToTable(nameof(UserLogins));
            builder.Entity<UserClaim>().ToTable(nameof(UserClaims));
            builder.Entity<UserToken>().ToTable(nameof(UserTokens));
        }
    }
}
