using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.EventBus.Interfaces.User
{
    public interface IUserRegisteredEvent
    {
        Guid UserId { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string Email { get; set; }
    }
}
