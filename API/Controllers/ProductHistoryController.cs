using Application.History;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;
public class ProductHistoryController : BaseApiController
{

    [AllowAnonymous]
    //[Authorize(Roles = "SuperAdmin,Admin,Employee")]
    [HttpGet("product")]
    public async Task<IActionResult> GetProductUpdateHistory()
    {
        return HandleResult(await Mediator.Send(new GetProductUpdateHistory.Query()));
    }

    [AllowAnonymous]
    [HttpGet("serialNumber")]
    public async Task<IActionResult> GetSerialNumberHistory()
    {
        return HandleResult(await Mediator.Send(new GetSerialNumberHistory.Query()));
    }


}
