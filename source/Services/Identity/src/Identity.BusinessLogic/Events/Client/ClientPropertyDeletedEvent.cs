using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client;

public class ClientPropertyDeletedEvent : AuditEvent
{
    public ClientPropertyDeletedEvent(ClientPropertiesDto clientProperty)
    {
        ClientProperty = clientProperty;
    }

    public ClientPropertiesDto ClientProperty { get; set; }
}