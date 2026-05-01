
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IAuditLogRepository
    {
        public Task InsertAsync(AuditLog auditLog);
    }
}
