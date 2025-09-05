using Identity.BusinessLogic.Identity.Helpers;

namespace Identity.BusinessLogic.Identity.Resources;

public class PersistedGrantAspNetIdentityServiceResources : IPersistedGrantAspNetIdentityServiceResources
{
    public virtual ResourceMessage PersistedGrantDoesNotExist() => new()
    {
        Code = nameof(PersistedGrantDoesNotExist),
        Description = PersistedGrantServiceResource.PersistedGrantDoesNotExist
    };

    public virtual ResourceMessage PersistedGrantWithSubjectIdDoesNotExist() => new()
    {
        Code = nameof(PersistedGrantWithSubjectIdDoesNotExist),
        Description = PersistedGrantServiceResource.PersistedGrantWithSubjectIdDoesNotExist
    };
}