using System;

namespace Orchestration.Core.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
}
