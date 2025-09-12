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
    public class AppUserStore<TContext> : UserStore<User, Role, TContext, Guid, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>
    where TContext : DbContext
    {
        public AppUserStore(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
        {
        }
    }
}
