

namespace Domain.Constants
{
    public class AuditLogConstants
    {
        public static class Actions
        {
            public const string CreateUser = "CREATE_USER";
            public const string ReserveSuccess = "RESERVE_SUCCESS";
            public const string ReserveAttemp = "RESERVE_ATTEMP";
            public const string ReserveExpired = "RESERVE_EXPIRED";
        }

        public static class Entities
        {
            public const string User = "User";
            public const string Seat = "Seat";
            public const string Reservation = "Reservation";
        }
    }
}
