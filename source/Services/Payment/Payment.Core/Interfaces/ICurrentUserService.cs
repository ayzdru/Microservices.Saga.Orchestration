using System;

namespace Payment.Core.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
}
