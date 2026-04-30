
using Application.UseCase._Seat.Commands.CreateSeatsForSector;

namespace Application.Interfaces.Handlers._Seat
{
    public interface ICreateSeatsForSectorHandler
    {
        public Task HandleAsync(IList<CreateSeatsForSectorCommand> command);
    }
}
