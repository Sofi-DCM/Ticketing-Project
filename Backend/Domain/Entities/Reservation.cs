
namespace Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public Guid SeatId { get; set; }
        public int StatusId { get; set; }
        public DateTime ReservedAt { get; set; }
        public DateTime ExpiredAt { get; set; }

        //---------
        public virtual User User { get; set; }
        //public Seat Seat { get; set; }
        
    }
}
