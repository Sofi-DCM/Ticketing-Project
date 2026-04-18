
namespace Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public int? UserId { get; set; }
        public int Action { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public int Details { get; set; }
        public DateTime CreatedAt { get; set; }

        //------
        public virtual User User { get; set; }
    }
}
