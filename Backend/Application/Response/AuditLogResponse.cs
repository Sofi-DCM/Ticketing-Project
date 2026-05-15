using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class AuditLogResponse
    {
        public int? UserId { get; set; }
        public string Action { get; set; } = null!;
        public string EntityType { get; set; } = null!;
        public string EntityId { get; set; } = null!;
        public string? Details { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
