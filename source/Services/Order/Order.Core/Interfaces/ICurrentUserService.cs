using System;

namespace Order.Core.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
}
