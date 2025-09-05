using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client;

public class ClientUpdatedEvent : AuditEvent
{
    public ClientUpdatedEvent(ClientDto originalClient, ClientDto client)
    {
        OriginalClient = originalClient;
        Client = client;
    }

    public ClientDto OriginalClient { get; set; }
    public ClientDto Client { get; set; }
}