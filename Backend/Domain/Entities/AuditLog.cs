
namespace Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public int? UserId { get; set; }
        public string Action { get; set; } = null!;
        public string EntityType { get; set; } = null!;
        public string EntityId { get; set; } = null!;
        public string? Details { get; set; }
        public DateTime CreatedAt { get; set; }

        //------
        public User? User { get; set; } 
    }
}
