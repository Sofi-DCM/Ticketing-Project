
using Application.UseCase._AuditLog.Queries.GetAuditLogsPaginated;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IAuditLogRepository
    {
        public Task InsertAllAsync(ICollection<AuditLog> auditLogs, CancellationToken ct);
        public Task<(ICollection<AuditLog>, int)> GetAuditLogsPaginated(GetAuditLogsPaginatedQuery queryDto, CancellationToken ct);
    }
}
