using Microsoft.AspNetCore.Identity;
using System;

namespace BuildingBlocks.Core.Entities;

public class UserClaim : IdentityUserClaim<Guid>
{
    public virtual User User { get; set; }
}