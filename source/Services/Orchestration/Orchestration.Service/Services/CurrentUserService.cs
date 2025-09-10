using BuildingBlocks.Core.Interfaces;
using Microsoft.AspNetCore.Http;


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
