using Microsoft.AspNetCore.Identity;
using System;

namespace BuildingBlocks.Core.Entities;

public class UserLogin : IdentityUserLogin<Guid>
{
    public virtual User User { get; set; }
}