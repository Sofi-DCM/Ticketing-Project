using Application.Interfaces.Handlers._AuditLog;

namespace Application.UseCase._AuditLog.Commands.CreateAuditLog
{
    public static class CreateAuditLogHandlerExtensions 
    {
        public static Task HandleAsync(
            this ICreateAuditLogHandler handler,
            CreateAuditLogCommand command,
            CancellationToken ct = default)
            => handler.HandleAsync([command], ct);
    }
}
