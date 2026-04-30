
namespace Application.UseCase._Seat.Commands.CreateSeatsForSector
{
    public class CreateSeatsForSectorCommand
    {
        public int SectorId { get; set; }
        public int ColumnsAmount { get; set; }
        public int RowsAmount { get; set; }
    }
}
