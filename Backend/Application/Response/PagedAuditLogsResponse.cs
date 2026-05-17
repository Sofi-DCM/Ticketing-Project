using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class PagedAuditLogsResponse
    {
        public ICollection<AuditLogResponse>? AuditLogs { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
