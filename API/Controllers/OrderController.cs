using Application.Orders;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;
public class OrderController : BaseApiController
{
    //[HttpPost("createOrder")] //api/GetCreateOrder
    //public async Task<IActionResult> CreateOrder(CreateOrderDto order)
    //{
    //    return HandleResult(await Mediator.Send(new CreateOrder.Command
    //    { Order = order }));
    //}

    [AllowAnonymous]
    [HttpPost("createOrder")] 
    public async Task<IActionResult> CreateOrder(CreateOrderDto order)
    {
        var orderId = Guid.NewGuid().ToString();

        var cookiesOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30) };

        Response.Cookies.Append("orderId", orderId, cookiesOptions);

        return HandleResult(await Mediator.Send(new CreateOrder.Command { Order = order, OrderId = orderId }));
    }

    [AllowAnonymous]
    [HttpPut]
    public async Task<IActionResult> UpdateOrder(UpdateOrderDto order)
    {
        return HandleResult(await Mediator.Send(new UpdateOrder.Command
        { Order = order }));
    }

    [Authorize(Roles = "SuperAdmin,Admin,Employee")]
    [HttpGet]
    public async Task<IActionResult> GetOrderList()
    {
        return HandleResult(await Mediator.Send(new OrderList.Query()));
    }

    [AllowAnonymous]
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderDetail(string orderId)
    {
        return HandleResult(await Mediator.Send(new GetOrderDetail.Query { OrderId = orderId }));
    }

    [AllowAnonymous]
    [HttpDelete("{orderId}")]
    public async Task<IActionResult> DeleteOrder(string orderId)
    {
        return HandleResult(await Mediator.Send(new DeleteOrder.Command { OrderId = orderId }));
    }

    [AllowAnonymous]
    [HttpDelete("{orderId}/{productName}")]
    public async Task<IActionResult> DeleteOrderDeatil(string orderId, string productName)
    {
        return HandleResult(await Mediator.Send(new DeleteOrderDetail.Command { OrderId = orderId, ProductName = productName }));
    }

}
