using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client;

public class ClientsRequestedEvent : AuditEvent
{
    public ClientsRequestedEvent(ClientsDto clientsDto)
    {
        ClientsDto = clientsDto;
    }

    public ClientsDto ClientsDto { get; set; }
}