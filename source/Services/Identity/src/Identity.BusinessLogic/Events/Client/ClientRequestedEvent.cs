using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client;

public class ClientRequestedEvent : AuditEvent
{
    public ClientRequestedEvent(ClientDto clientDto)
    {
        ClientDto = clientDto;
    }

    public ClientDto ClientDto { get; set; }
}