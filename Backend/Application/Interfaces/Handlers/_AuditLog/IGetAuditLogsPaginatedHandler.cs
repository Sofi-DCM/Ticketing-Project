using Application.Response;
using Application.UseCase._AuditLog.Queries.GetAuditLogsPaginated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Handlers._AuditLog
{
    public interface IGetAuditLogsPaginatedHandler
    {
        public Task<PagedAuditLogsResponse> HandleAsync(GetAuditLogsPaginatedQuery query, CancellationToken ct);
    }
}
