using Identity.BusinessLogic.Helpers;

namespace Identity.BusinessLogic.Resources;

public class PersistedGrantServiceResources : IPersistedGrantServiceResources
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