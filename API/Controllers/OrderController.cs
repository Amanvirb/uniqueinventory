using Application.Consolidations;
using Application.Orders;

namespace API.Controllers
{
    public class OrderController : BaseApiController
    {
        [HttpPost] //api/GetOrder
        public async Task<IActionResult> GetOrder(OrderRoutePayloadDto[] order)
        {
            return HandleResult(await Mediator.Send(new OrderRoute.Query
            { Order = order }));
        }
    }
}
