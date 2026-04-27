namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/sectors")]
    public class SectorsController : ControllerBase
    {
        private readonly IGetSectorsByEventIdHandler _getSectorsByEventIdHandler;

        public SectorsController(IGetSectorsByEventIdHandler getSectorsByEventIdHandler)
        {
            _getSectorsByEventIdHandler = getSectorsByEventIdHandler;
        }

        [HttpGet("{eventId}")]
        [ProducesResponseType(typeof(List<SectorResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSectorsByEventId(int eventId, CancellationToken ct)
        {
            var response = await _getSectorsByEventIdHandler.HandleAsync(eventId, ct);

            return Ok(response);
        }
    }
}
