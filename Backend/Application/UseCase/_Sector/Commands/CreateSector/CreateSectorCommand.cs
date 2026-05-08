
namespace Application.UseCase._Sector.Commands.CreateSector
{
    public class CreateSectorCommand
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int ColumnsAmount { get; set; }
        public int RowsAmount { get; set; }
    }
}
