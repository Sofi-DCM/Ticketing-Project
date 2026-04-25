namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatsController : ControllerBase
    {
        private readonly IGetSeatsBySectorHandler _getSeatsBySectorHandler;

        public SeatsController(IGetSeatsBySectorHandler getSeatsBySectorHandler)
        {
            _getSeatsBySectorHandler = getSeatsBySectorHandler;
        }

        [HttpGet("{sectorId}")]
        [ProducesResponseType(typeof(List<SeatStatusDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSeatsBySector(
            int sectorId,
            CancellationToken ct)
        {
            var response = await _getSeatsBySectorHandler.HandleAsync(sectorId, ct);

            return Ok(response);
        }
    }
}
