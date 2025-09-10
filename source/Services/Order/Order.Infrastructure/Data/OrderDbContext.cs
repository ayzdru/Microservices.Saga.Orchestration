using BuildingBlocks.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Order.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Order.Infrastructure.Data
{
    public class OrderDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>, IApplicationDbContext
    {
        public DbSet<Order.Core.Entities.Order> Orders { get; set; }
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
             : base(options)
        {

        }      

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
            ConfigureIdentity(builder);
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
