using Application.Consolidations;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[Authorize(Roles = "SuperAdmin,Admin")]
public class ConsolidationController : BaseApiController
{
    [HttpGet("Pick")]
    public async Task<IActionResult> GetConsolidatedLocations([FromQuery] SearchParams searchParams)
    {
        return HandleResult(await Mediator.Send(new GeneratePickConsolidations.Query
        { SearchParams = searchParams }));
    }

    [HttpGet("Put")]
    public async Task<IActionResult> GetConsolidatedPutLocations([FromQuery] SearchParams searchParams)
    {
        return HandleResult(await Mediator.Send(new GeneratePutConsolidations.Query
        { SearchParams = searchParams }));
    }
}
