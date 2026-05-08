
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using System.Threading;

namespace Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly AppDbContext _context;

        public AuditLogRepository(AppDbContext context)
        {
            _context=context;
        }

        public async Task InsertAllAsync(ICollection<AuditLog> auditLogs, CancellationToken ct) 
        {
            _context.AuditLogs.AddRange(auditLogs);
            await _context.SaveChangesAsync();
        }
    }
}
