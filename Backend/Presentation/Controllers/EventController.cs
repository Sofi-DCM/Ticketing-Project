
namespace Presentation.Controllers
{
    [Route("api/v1/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IGetActiveEventsHandler _getActiveEventsHandler;
        private readonly ICreateEventHandler _createEventHandler;

        public EventController(IGetActiveEventsHandler getActiveEventsHandler, ICreateEventHandler createEventHandler)
        {
            _getActiveEventsHandler = getActiveEventsHandler;
            _createEventHandler = createEventHandler;
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

    }
}
