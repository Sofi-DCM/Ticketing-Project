namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/seats")]
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
            bool? onlyRow,
            CancellationToken ct)
        {
            var response = await _getSeatsBySectorHandler.HandleAsync(sectorId, onlyRow, ct);

            return Ok(response);
        }
    }
}
