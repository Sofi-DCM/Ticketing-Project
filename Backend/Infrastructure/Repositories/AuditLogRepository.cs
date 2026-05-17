
using Application.Interfaces.Repositories;
using Application.UseCase._AuditLog.Queries.GetAuditLogsPaginated;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Threading;

namespace Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly AppDbContext _context;

        public AuditLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertAllAsync(ICollection<AuditLog> auditLogs, CancellationToken ct) 
        {
            _context.AuditLogs.AddRange(auditLogs);
            await _context.SaveChangesAsync();
        }

        public async Task<(ICollection<AuditLog>, int)> GetAuditLogsPaginated(GetAuditLogsPaginatedQuery queryDto, CancellationToken ct)
        {
            var query = _context.AuditLogs.AsNoTracking();

            query =query
                .Where(a => (!queryDto.UserId.HasValue || a.UserId == queryDto.UserId))
                .Where(a => (string.IsNullOrEmpty(queryDto.EntityId) || a.EntityId == queryDto.EntityId))

                .Where(a => (!queryDto.CreatedAt.HasValue || a.CreatedAt.Date == queryDto.CreatedAt.Value.Date))
                .Where(a => (!queryDto.StartDate.HasValue || a.CreatedAt >= queryDto.StartDate))
                .Where(a => (!queryDto.EndDate.HasValue || a.CreatedAt <= queryDto.EndDate))

                .Where(a => (string.IsNullOrEmpty(queryDto.EntityType) || a.EntityType == queryDto.EntityType))
                .Where(a => (string.IsNullOrEmpty(queryDto.Action) || a.Action == queryDto.Action));

            
            var TotalCount = await query.CountAsync(ct);
            var audits = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((queryDto.PageNumber - 1) * queryDto.PageSize)
                .Take(queryDto.PageSize)
                .ToListAsync(ct);

            return (audits, TotalCount);
        }
    }
}
