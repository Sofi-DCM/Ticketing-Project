

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        //-------------
        public List<Reservation>? Reservations { get; set; }
        public virtual List<AuditLog> AuditLog { get; set; }
    }
}
