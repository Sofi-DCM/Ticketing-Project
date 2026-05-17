
namespace Application.Response
{
    public class UserReservationResponse
    {
        public Guid ReservationId { get; set; }
        public string SeatName { get; set; } = null!;
        public string EventName { get; set; } = null!;
        public string SectorName { get; set; } = null!;
        public decimal SectorPrice { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
