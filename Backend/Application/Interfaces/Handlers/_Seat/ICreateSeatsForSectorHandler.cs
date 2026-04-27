using Application.UseCase._Seat.Commands.CreateSeatsForSector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Handlers._Seat
{
    public interface ICreateSeatsForSectorHandler
    {
        public Task HandleAsync(IList<CreateSeatsForSectorCommand> command);
    }
}
