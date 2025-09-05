using System.Collections.Generic;
using Identity.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Identity.BusinessLogic.Identity.Dtos.Identity.Base;

public class BaseUserDto<TUserId> : IBaseUserDto
{
    public TUserId Id { get; set; }

    public bool IsDefaultId() => EqualityComparer<TUserId>.Default.Equals(Id, default);

    object IBaseUserDto.Id => Id;
}