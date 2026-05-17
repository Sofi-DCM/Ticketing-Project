
using Application.Interfaces;
using Application.Interfaces.Handlers._Event;
using Application.Interfaces.Handlers._Sector;
using Application.Interfaces.Repositories;
using Domain.Constants;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCase._Event.Commands.CreateEvent
{
    public class CreateEventHandler : ICreateEventHandler
    {
        private readonly IEventRepository _eventRepository;
        private readonly ICreateSectorHandler _sectorHandler;
        private readonly IUnitOfWork _unitOfWork;
        public CreateEventHandler(IEventRepository eventRepository, ICreateSectorHandler sectorHandler, IUnitOfWork unitOfWork)
        {
            _eventRepository = eventRepository;
            _sectorHandler = sectorHandler;
            _unitOfWork = unitOfWork;
        }
        public async Task<int> HandleAsync(CreateEventCommand command, CancellationToken ct)
        {
            if (command.UserId <= 0)
                throw new ArgumentException("Los id deben ser positivos");

            if (command.UserId != 1)
                throw new UnauthorizedException("Solo el administrador puede crear eventos.");

            if (command.EventDate <= DateTime.UtcNow)
                throw new ArgumentException("El evento debe tener una fecha futura.");

            if (command.SectorsCommands == null || !command.SectorsCommands.Any())
                throw new ArgumentException("El evento debe tener al menos un sector definido.");

            if (command.SectorsCommands.Any(s => s.RowsAmount <= 0 || s.ColumnsAmount <= 0))
                throw new ArgumentException("Todos los sectores deben tener al menos 1 fila y 1 columna.");

            if (command.SectorsCommands.Any(s => s.Price <= 0))
                throw new ArgumentException("Todos los sectores deben tener precio");

            var newEvent = new Event
            {
                Name = command.Name,
                EventDate = command.EventDate,
                Venue = command.Venue,
                Status = EventStatusConstants.Active
            };

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var newEventId = await _eventRepository.InsertEventAsync(newEvent, ct);

                await _sectorHandler.HandleAsync(command.SectorsCommands, newEventId);

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();  
                
                return newEventId;
            }
            catch (Exception) {
                await transaction.RollbackAsync();    
                throw;
            }
        }
    }
}
