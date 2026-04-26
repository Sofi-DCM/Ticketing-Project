using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase._Seat.Commands.CreateSeatsForSector
{
    public class CreateSeatsForSectorCommand
    {
        public int SectorId { get; set; }
        public int ColumnsAmount { get; set; }
        public int RowsAmount { get; set; }
    }
}
