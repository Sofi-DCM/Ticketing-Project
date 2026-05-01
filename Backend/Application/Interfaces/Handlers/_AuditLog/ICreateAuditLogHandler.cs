
using Application.UseCase._AuditLog.Commands.CreateAuditLog;

namespace Application.Interfaces.Handlers._AuditLog
{
    public interface ICreateAuditLogHandler
    {
        public Task HandleAsync(CreateAuditLogCommand command);
    }
}
