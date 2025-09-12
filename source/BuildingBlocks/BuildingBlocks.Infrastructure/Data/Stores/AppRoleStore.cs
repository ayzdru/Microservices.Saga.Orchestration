using BuildingBlocks.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Infrastructure.Data.Stores
{
    public class AppRoleStore<TContext> : RoleStore<Role, TContext, Guid, UserRole, RoleClaim>
        where TContext : DbContext
    {
        public AppRoleStore(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
        {
        }
    }
}
