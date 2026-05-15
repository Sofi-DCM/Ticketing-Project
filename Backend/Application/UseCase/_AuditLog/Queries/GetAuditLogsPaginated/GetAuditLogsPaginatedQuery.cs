

namespace Application.UseCase._AuditLog.Queries.GetAuditLogsPaginated
{
    public class GetAuditLogsPaginatedQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public int? UserId { get; set; }
        public string? Action { get; set; }
        public string? EntityType { get; set; }
        public string? EntityId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
