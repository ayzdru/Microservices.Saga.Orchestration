using AutoMapper;
using Identity.BusinessLogic.Dtos.Log;
using Identity.EntityFramework.Entities;
using Identity.EntityFramework.Extensions.Common;
using Identity.AuditLogging.EntityFramework.Entities;

namespace Identity.BusinessLogic.Mappers;

public static class LogMappers
{
    static LogMappers()
    {
        Mapper = new MapperConfiguration(cfg => cfg.AddProfile<LogMapperProfile>())
            .CreateMapper();
    }

    internal static IMapper Mapper { get; }

    public static LogDto ToModel(this Log log) => Mapper.Map<LogDto>(log);

    public static LogsDto ToModel(this PagedList<Log> logs) => Mapper.Map<LogsDto>(logs);

    public static AuditLogsDto ToModel<TAuditLog>(this PagedList<TAuditLog> auditLogs)
        where TAuditLog : AuditLog
        => Mapper.Map<AuditLogsDto>(auditLogs);

    public static AuditLogDto ToModel(this AuditLog auditLog) => Mapper.Map<AuditLogDto>(auditLog);

    public static Log ToEntity(this LogDto log) => Mapper.Map<Log>(log);
}