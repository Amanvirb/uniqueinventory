using Application.Orders;
using Application.Search;

namespace API.Controllers
{
    public class SearchProductController : BaseApiController
    {

        [HttpGet("{Name}")] //api/GetSerachedProducts
        public async Task<IActionResult> GetSearchedProducts(string name)
        {
            return HandleResult(await Mediator.Send(new SearchProduct.Query { Name = name}));
        }
    }
}
