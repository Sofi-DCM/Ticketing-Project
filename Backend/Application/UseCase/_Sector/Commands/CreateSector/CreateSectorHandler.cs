
using Application.Interfaces.Handlers._Seat;
using Application.Interfaces.Handlers._Sector;
using Application.Interfaces.Repositories;
using Application.UseCase._Seat.Commands.CreateSeatsForSector;
using Domain.Entities;

namespace Application.UseCase._Sector.Commands.CreateSector
{
    public class CreateSectorHandler : ICreateSectorHandler
    {
        private readonly ISectorRepository _repository;
        private readonly ICreateSeatsForSectorHandler _seatsForSectorHandler;

        public CreateSectorHandler(ISectorRepository repository, ICreateSeatsForSectorHandler seatsForSectorHandler)
        {
            _repository = repository;
            _seatsForSectorHandler = seatsForSectorHandler;
        }

        public async Task HandleAsync(IList<CreateSectorCommand> sectorCommands, int eventId)
        {
            ICollection<Sector> sectors = sectorCommands.Select(command => new Sector
            {
                EventId = eventId,
                Name = command.Name,
                Price = command.Price,
                Capacity = command.ColumnsAmount * command.RowsAmount
            }).ToList();

            var newSectorsIds = await _repository.InsertAsync(sectors);

            IList<CreateSeatsForSectorCommand> seatsCommands = [];

            for (int i = 0; i < sectorCommands.Count(); i++) 
            {
                seatsCommands.Add(new CreateSeatsForSectorCommand { 
                    SectorId = newSectorsIds[i],
                    ColumnsAmount = sectorCommands[i].ColumnsAmount,
                    RowsAmount = sectorCommands[i].RowsAmount,
                });
            };

            await _seatsForSectorHandler.HandleAsync(seatsCommands);

        }
    }
}
