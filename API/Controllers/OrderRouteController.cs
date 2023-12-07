using Application.Consolidations;
using Application.Locations;
using Application.Orders;
using Domain;

namespace API.Controllers
{
    public class OrderRouteController : BaseApiController
    {
        [HttpGet("{id}")] //api/GetOrderRoute
        public async Task<IActionResult> GetOrderRoute(int id)
        {
            return HandleResult(await Mediator.Send(new OrderRoute.Query
            { Id = id }));
        }
    }
}
