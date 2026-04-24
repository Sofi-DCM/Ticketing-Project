using Application.Interfaces.Handlers._AuditLog;
using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
