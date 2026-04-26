namespace Presentation.Controllers
{
        [ApiController]
        [Route("api/v1/sectors")]
        public class SectorController : ControllerBase
        {
            private readonly IGetSectorsByEventIdHandler _getSectorsByEventIdHandler;

            public SectorController(IGetSectorsByEventIdHandler getSectorsByEventIdHandler)
            {
                _getSectorsByEventIdHandler = getSectorsByEventIdHandler;
            }

            [HttpGet("event/{eventId}")]
            public async Task<IActionResult> GetSectorsByEventId(int eventId, CancellationToken ct)
            {
                var response = await _getSectorsByEventIdHandler.HandleAsync(eventId, ct);
                return Ok(response);
            }
        }
}
