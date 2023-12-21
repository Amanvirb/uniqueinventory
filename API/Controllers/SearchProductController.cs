using Application.ProductSearch;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[AllowAnonymous]
public class SearchProductController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetSearchedProducts([FromQuery] ProductSearchParams searchParams)
    {
        return HandleResult(await Mediator.Send(new SearchProduct.Query { Params = searchParams }));
    }
}
