using Application.Consolidations;
using Application.Locations;

namespace API.Controllers
{
    public class ConsolidationController : BaseApiController
    {
        [HttpGet("{maxUnit}/{partNumberName}")] //api/GetProductDetail
        public async Task<IActionResult> GetLocationDetail(int maxUnit, string partNumberName)
        {
            return HandleResult(await Mediator.Send(new GenerateConsolidations.Query { MaxUnit = maxUnit, PartNumberName = partNumberName }));
        }
    }
}
