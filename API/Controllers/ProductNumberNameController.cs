using Application.Locations;
using Application.ProductNumbers;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

public class ProductNumberNameController : BaseApiController
{

    //[Authorize(Roles = "SuperAdmin,Admin,Employee")]
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateProductNumber(CommonDto name)
    {
        return HandleResult(await Mediator.Send(new AddProductNumber.Command { Name = name }));
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetProductNumberNameList()
    {
        return HandleResult(await Mediator.Send(new ProductNumberNameList.Query()));
    }

    [AllowAnonymous]
    [HttpGet("{name}")]
    public async Task<IActionResult> GetProductNameDetail(string name)
    {
        return HandleResult(await Mediator.Send(new ProductNameDetail.Query { Name = name }));
    }

    [Authorize(Roles = "SuperAdmin,Admin,Employee")]
    [HttpPut]
    public async Task<IActionResult> UpdateProductNameDetail(CommonDto product)
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
