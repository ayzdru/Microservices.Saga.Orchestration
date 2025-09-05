using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client;

public class ClientAddedEvent : AuditEvent
{
    public ClientAddedEvent(ClientDto client)
    {
        Client = client;
    }

    public ClientDto Client { get; set; }
}