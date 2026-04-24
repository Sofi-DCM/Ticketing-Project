using Application.Response;
using Application.UseCase._Event.Queries.GetActiveEvents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/v1/[controller]")]
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
