using System;

namespace BuildingBlocks.Core.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
}
