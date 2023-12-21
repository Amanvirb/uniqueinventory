using Application.Orders;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;
public class OrderRouteController : BaseApiController
{
    [Authorize(Roles = "SuperAdmin,Admin,Employee")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderRoute(string id)
    {
        return HandleResult(await Mediator.Send(new OrderRoute.Query
        { OrderId = id }));
    }
}
