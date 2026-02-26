using System;
using System.Collections.Generic;
using System.Text;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Log;

public class LogsDeletedEvent : AuditEvent
{
    public LogsDeletedEvent(DateTime deleteOlderThan)
    {
        DeleteOlderThan = deleteOlderThan;
    }

    public DateTime DeleteOlderThan { get; set; }
}
