using Application.Consolidations;
using Application.Locations;

namespace API.Controllers
{
    public class ConsolidationController : BaseApiController
    {
        [HttpGet] //api/GetConsolidatedLocations
        public async Task<IActionResult> GetConsolidatedLocations([FromQuery] SearchParams searchParams)
        {
            return HandleResult(await Mediator.Send(new GenerateConsolidations.Query
            { SearchParams = searchParams }));
        }
    }
}
