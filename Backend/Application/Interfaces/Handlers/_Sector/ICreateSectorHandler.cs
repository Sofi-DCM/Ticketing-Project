
using Application.UseCase._Sector.Commands.CreateSector;

namespace Application.Interfaces.Handlers._Sector
{
    public interface ICreateSectorHandler
    {
        public Task HandleAsync(IList<CreateSectorCommand> sectorCommands, int eventId);
    }
}
