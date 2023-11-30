using Application.Consolidations;

namespace API.Controllers
{
    public class ConsolidationController : BaseApiController
    {
        [HttpGet("Pick")] //api/GetConsolidatedPickLocations
        public async Task<IActionResult> GetConsolidatedLocations([FromQuery] SearchParams searchParams)
        {
            return HandleResult(await Mediator.Send(new GeneratePickConsolidations.Query
            { SearchParams = searchParams }));
        }

        [HttpGet("Put")] //api/GetConsolidatedPutLocations
        public async Task<IActionResult> GetConsolidatedPutLocations([FromQuery] SearchParams searchParams)
        {
            return HandleResult(await Mediator.Send(new GeneratePutConsolidations.Query
            { SearchParams = searchParams }));
        }
    }
}
