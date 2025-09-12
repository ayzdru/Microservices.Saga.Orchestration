using Microsoft.AspNetCore.Identity;
using System;

namespace BuildingBlocks.Core.Entities;

public class UserToken : IdentityUserToken<Guid>
{
    public virtual User User { get; set; }
}