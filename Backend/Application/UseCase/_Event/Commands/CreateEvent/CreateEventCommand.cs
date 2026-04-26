using Application.UseCase._Sector.Commands.CreateSector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase._Event.Commands.CreateEvent
{
    public class CreateEventCommand
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Venue { get; set; } = string.Empty;

        public IList<CreateSectorCommand> SectorsCommands { get; set; } = new List<CreateSectorCommand>();

    }
}
