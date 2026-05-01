
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

        public async Task HandleAsync(CreateAuditLogCommand command)
        {
            var newAudit = new AuditLog 
            {
                UserId = command.UserId,
                Action = command.Action,
                EntityType = command.EntityType,
                EntityId = command.EntityId,
                Details = command.Details,
                CreatedAt = DateTime.Now,
            };

            await _repository.InsertAsync(newAudit);
        }
    }
}
