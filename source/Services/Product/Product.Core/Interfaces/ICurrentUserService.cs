using System;

namespace Product.Core.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
}
