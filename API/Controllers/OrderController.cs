using Application.Consolidations;
using Application.Locations;
using Application.Orders;
using Domain;

namespace API.Controllers
{
    public class OrderController : BaseApiController
    {
        [HttpPost("OrderRoute")] //api/GetOrderRoute
        public async Task<IActionResult> GetOrderRoute(OrderRoutePayloadDto[] order)
        {
            return HandleResult(await Mediator.Send(new OrderRoute.Query
            { Order = order }));
        }

        [HttpPost("createOrder")] //api/GetCreateOrder
        public async Task<IActionResult> CreateOrder(CreateOrderDto order)
        {
            return HandleResult(await Mediator.Send(new CreateOrder.Command
            { Order = order }));
        }

        [HttpPut] //api/UpdateOrder
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto order)
        {
            return HandleResult(await Mediator.Send(new UpdateOrder.Command
            { Order = order }));
        }

        [HttpGet] //api/GetOrderList
        
        public async Task<IActionResult> GetOrderList()
        {
            return HandleResult(await Mediator.Send(new OrderList.Query()));
        }
    }
}
