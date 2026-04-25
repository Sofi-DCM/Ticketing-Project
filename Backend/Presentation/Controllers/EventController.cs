namespace Presentation.Controllers
{
    [Route("api/v1/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IGetActiveEventsHandler _getActiveEventsHandler;

        public EventController(IGetActiveEventsHandler getActiveEventsHandler)
        {
            _getActiveEventsHandler=getActiveEventsHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveEvents([FromQuery] GetActiveEventsQuery query, CancellationToken ct = default)
        {
            var response = await _getActiveEventsHandler.HandleAsync(query, ct);

            return Ok(response);
        }
    }
}
