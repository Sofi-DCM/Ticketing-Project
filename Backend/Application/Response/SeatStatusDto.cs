
namespace Application.Response
{
    public class SeatStatusDto
    {
        public Guid Id { get; set; }
        public string RowIdentifier { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
