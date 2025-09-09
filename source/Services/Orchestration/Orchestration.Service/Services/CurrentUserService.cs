using Microsoft.AspNetCore.Http;
using Orchestration.Core.Interfaces;



namespace Orchestration.API.Services;

public class CurrentUserService : ICurrentUserService
{

    public CurrentUserService()
    {
    }
    public Guid? UserId
    {
        get
        {
            
            return null;
        }
    }
}
