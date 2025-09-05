using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client;

public class ClientClonedEvent : AuditEvent
{
    public ClientClonedEvent(ClientCloneDto client)
    {
        Client = client;
    }

    public ClientCloneDto Client { get; set; }
}