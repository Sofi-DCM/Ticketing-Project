using Application.Interfaces.Handlers._Event;
using Application.Interfaces.Repositories;
using Domain.Constants;
using Domain.Entities;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase._Event.Commands.CreateEvent
{
    public class CreateEventHandler : ICreateEventHandler
    {
        private readonly IEventRepository _eventRepository;
        public CreateEventHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public async Task<int> HandleAsync(CreateEventCommand command, CancellationToken ct)
        {
            if (command.UserId != 1)
                throw new UnauthorizedException("Solo el administrador puede crear eventos.");

            var newEvent = new Event
            {
                Name = command.name,
                EventDate = command.EventDate,
                Venue = command.Venue,
                Status = EventStatusConstants.Active
            };
            return await _eventRepository.InsertEventAsync(newEvent, ct);
        }
    }
}
