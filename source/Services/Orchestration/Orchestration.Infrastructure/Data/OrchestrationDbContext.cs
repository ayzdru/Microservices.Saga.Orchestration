using BuildingBlocks.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Orchestration.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orchestration.Infrastructure.Data
{
    public class OrchestrationDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>, IApplicationDbContext
    {
        public OrchestrationDbContext(DbContextOptions<OrchestrationDbContext> options)
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
