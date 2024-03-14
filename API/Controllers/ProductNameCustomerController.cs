using Application.ProductNames;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

public class ProductNameCustomerController : BaseApiController
{

    

    [AllowAnonymous]
    [HttpGet]
    //public async Task<IActionResult> GetProductNameList()
    public async Task<IActionResult> GetProductNameList([FromQuery] ProductNameSearchParams searchParams)

    {
        return HandlePagedResult(await Mediator.Send(new ProductNameList.Query { Params= searchParams}));
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductNameDetail(int id)
    {
        return HandleResult(await Mediator.Send(new ProductNameDetail.Query { Id = id }));
    }

   

}
