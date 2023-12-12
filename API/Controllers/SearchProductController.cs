using Application.Orders;
using Application.ProductSearch;

namespace API.Controllers
{
    public class SearchProductController : BaseApiController
    {
        [HttpGet] //api/GetSerachedProducts
        public async Task<IActionResult> GetSearchedProducts([FromQuery]ProductSearchParams searchParams)
        {
            return HandleResult(await Mediator.Send(new SearchProduct.Query { Params = searchParams}));
        }
    }
}
