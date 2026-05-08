
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IAuditLogRepository
    {
        public Task InsertAllAsync(ICollection<AuditLog> auditLogs, CancellationToken ct);
    }
}
