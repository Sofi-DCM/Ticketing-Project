using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/v1/auditlogs")]
    [ApiController]
    public class AuditLogController : ControllerBase
    {
        private readonly IGetAuditLogsPaginatedHandler _getAuditLogsPaginatedHandler;

        public AuditLogController(IGetAuditLogsPaginatedHandler getAuditLogsPaginatedHandler)
        {
            _getAuditLogsPaginatedHandler = getAuditLogsPaginatedHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuditLogsPaginated([FromQuery]GetAuditLogsPaginatedQuery query, CancellationToken ct = default)
        {
            var response = await _getAuditLogsPaginatedHandler.HandleAsync(query, ct);

            return Ok(response);
        }
    }
}
