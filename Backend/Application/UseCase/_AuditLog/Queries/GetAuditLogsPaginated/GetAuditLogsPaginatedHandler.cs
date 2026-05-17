using Application.Interfaces.Handlers._AuditLog;
using Application.Interfaces.Repositories;
using Application.Response;
using Application.UseCase._Event.Queries.GetActiveEvents;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase._AuditLog.Queries.GetAuditLogsPaginated
{
    public class GetAuditLogsPaginatedHandler : IGetAuditLogsPaginatedHandler
    {
        private readonly IAuditLogRepository _repository;

        public GetAuditLogsPaginatedHandler(IAuditLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedAuditLogsResponse> HandleAsync(GetAuditLogsPaginatedQuery query, CancellationToken ct)
        {
            if (query.PageNumber < 1)
                throw new ArgumentException("La pagina solicitada no puede ser menor a 1");

            if (query.PageSize < 1 || query.PageSize > 50)
                throw new ArgumentException("El tamaño de página debe estar entre 1 y 50.");

            var response = await _repository.GetAuditLogsPaginated(query, ct);

            return MapToPagedAuditLogsResponse(response, query);
        }

        private static PagedAuditLogsResponse MapToPagedAuditLogsResponse((ICollection<AuditLog> auditLogs, int total) response, GetAuditLogsPaginatedQuery query)
        {
            var lastPageNumber = (int)Math.Ceiling(response.total / (double)query.PageSize);

            return new PagedAuditLogsResponse
            {
                AuditLogs = response.auditLogs?.Select(a => new AuditLogResponse
                {
                    UserId = a.UserId,
                    Action = a.Action,
                    EntityType = a.EntityType,
                    EntityId = a.EntityId,
                    Details = a.Details,
                    CreatedAt = a.CreatedAt,
                }).ToList() ?? new List<AuditLogResponse>(),
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                HasPreviousPage = query.PageNumber > 1,
                HasNextPage = query.PageNumber < lastPageNumber,
            };
        }
    }
}

