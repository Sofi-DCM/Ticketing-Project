using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCase._AuditLog.Commands;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IAuditLogRepository
    {
        public Task InsertAsync(AuditLog auditLog);
    }
}
