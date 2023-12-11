using Application.Consolidations;
using Application.Locations;
using Application.Orders;
using Domain;

namespace API.Controllers
{
    public class OrderController : BaseApiController
    {
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

        [HttpGet("{id}")] //api/GetOrderDetail
        public async Task<IActionResult> GetOrderDetail(int id)
        {
            return HandleResult(await Mediator.Send(new GetOrderDetail.Query { Id = id} ));
        }

        [HttpDelete("{id}")] //api/GetOrderDetail
        public async Task<IActionResult> DeleteOrder(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteOrder.Command { Id = id }));
        }
        
        [HttpDelete("{productName}, {id}")] //api/GetOrderDetail
        public async Task<IActionResult> DeleteOrderDeatil(string productName, int id)
        {
            return HandleResult(await Mediator.Send(new DeleteOrderDetail.Command {ProductName = productName, OrderId = id }));
        }

    }
}
