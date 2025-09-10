using BuildingBlocks.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Payment.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Data
{
    public class PaymentDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>, IApplicationDbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
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
