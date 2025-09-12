using Microsoft.AspNetCore.Identity;
using System;

namespace BuildingBlocks.Core.Entities;

public class UserRole : IdentityUserRole<Guid>
{
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
}