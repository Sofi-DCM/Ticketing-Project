
using Application.Interfaces.Handlers._AuditLog;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.UseCase._AuditLog.Commands.CreateAuditLog
{
    public class CreateAuditLogHandler : ICreateAuditLogHandler
    {
        private readonly IAuditLogRepository _repository;

        public CreateAuditLogHandler(IAuditLogRepository repository)
        {
            _repository=repository;
        }

        public async Task HandleAsync(IReadOnlyList<CreateAuditLogCommand> commands, CancellationToken ct = default)
        {
            if (!commands.Any()) return;

            var newAuditLogs = commands.Select(MapToAuditLogEntity).ToList();

            await _repository.InsertAllAsync(newAuditLogs, ct);
        }

        private static AuditLog MapToAuditLogEntity(CreateAuditLogCommand command) { 
            return new AuditLog
            {
                UserId = command.UserId,
                Action = command.Action,
                EntityType = command.EntityType,
                EntityId = command.EntityId,
                Details = command.Details,
                CreatedAt = DateTime.Now,
            };
        }
    }
}
