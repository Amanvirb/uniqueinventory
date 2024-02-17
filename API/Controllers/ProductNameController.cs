using Application.Locations;
using Application.ProductNames;
using Application.ProductNames.Dtoæ;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

public class ProductNameController : BaseApiController
{

    //[Authorize(Roles = "SuperAdmin,Admin,Employee")]
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateProductName(AddProductNameDto name)
    {
        return HandleResult(await Mediator.Send(new AddProductName.Command { Product = name }));
    }

    [AllowAnonymous]
    [HttpGet]
    //public async Task<IActionResult> GetProductNameList()
    public async Task<IActionResult> GetProductNameList([FromQuery] ProductNameSearchParams searchParams)

    {
        return HandleResult(await Mediator.Send(new ProductNameList.Query { Params= searchParams}));
    }

    [AllowAnonymous]
    [HttpGet("{name}")]
    public async Task<IActionResult> GetProductNameDetail(string name)
    {
        return HandleResult(await Mediator.Send(new ProductNameDetail.Query { Name = name }));
    }

    [Authorize(Roles = "SuperAdmin,Admin,Employee")]
    [HttpPut]
    public async Task<IActionResult> UpdateProductNameDetail(AddProductNameDto product)
    {
        return HandleResult(await Mediator.Send(new EditProductName.Command { Product = product }));
    }

    [Authorize(Roles = "SuperAdmin,Admin,Employee")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductName(int id)
    {
        return HandleResult(await Mediator.Send(new DeleteProductName.Command { Id = id }));
    }

}
