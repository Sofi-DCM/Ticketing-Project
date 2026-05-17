
using Application.UseCase._AuditLog.Commands.CreateAuditLog;

namespace Application.Interfaces.Handlers._AuditLog
{
    public interface ICreateAuditLogHandler
    {
        public Task HandleAsync(IReadOnlyList<CreateAuditLogCommand> commands, CancellationToken ct);
    }
}
