using Application.UseCase._AuditLog.Commands.CreateAuditLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Handlers._AuditLog
{
    public interface ICreateAuditLogHandler
    {
        public Task HandleAsync(CreateAuditLogCommand command);
    }
}
