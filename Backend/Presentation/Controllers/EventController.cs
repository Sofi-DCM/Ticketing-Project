
namespace Presentation.Controllers
{
    [Route("api/v1/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IGetActiveEventsHandler _getActiveEventsHandler;
        private readonly ICreateEventHandler _createEventHandler;
        private readonly IGetEventByIdHandler _getEventByIdHandler;

        public EventController
            (IGetActiveEventsHandler getActiveEventsHandler, 
            ICreateEventHandler createEventHandler, 
            IGetEventByIdHandler getEventByIdHandler)
        {
            _getActiveEventsHandler = getActiveEventsHandler;
            _createEventHandler = createEventHandler;
            _getEventByIdHandler = getEventByIdHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveEvents([FromQuery] GetActiveEventsQuery query, CancellationToken ct = default)
        {
            var response = await _getActiveEventsHandler.HandleAsync(query, ct);

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventCommand command, CancellationToken ct)
        {
            var id = await _createEventHandler.HandleAsync(command, ct);
            return Created(string.Empty, new { eventId = id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id, CancellationToken ct = default) 
        {
            var response = await _getEventByIdHandler.HandleAsync(id);
            return Ok(response);
        }
    }
}
