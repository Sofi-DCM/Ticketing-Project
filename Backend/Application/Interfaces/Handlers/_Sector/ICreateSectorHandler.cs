using Application.UseCase._Sector.Commands.CreateSector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Handlers._Sector
{
    public interface ICreateSectorHandler
    {
        public Task HandleAsync(IList<CreateSectorCommand> sectorCommands, int eventId);
    }
}
