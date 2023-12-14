using Application.Dto;
using Application.Locations;
using Application.ProductNumbers;
using Domain;

namespace API.Controllers;
public class ProductNumberNameController : BaseApiController
{
    [HttpPost] //api//CreateLocation
    public async Task<IActionResult> CreateProductNumber(CommonDto name)
    {
        return HandleResult(await Mediator.Send(new AddProductNumber.Command { Name = name }));
    }

    [HttpGet] //api/ GetProductNumberNameList
    public async Task<IActionResult> GetProductNumberNameList()
    {
        return HandleResult(await Mediator.Send(new ProductNumberNameList.Query()));
    }

    [HttpGet("{name}")] //api/GetProductDetail
    public async Task<IActionResult> GetProductNameDetail(string name)
    {
        return HandleResult(await Mediator.Send(new ProductNameDetail.Query { Name = name }));
    }

    [HttpPut] //api/UpdateProductDetail
    public async Task<IActionResult> UpdateProductNameDetail(CommonDto product)
    {
        return HandleResult(await Mediator.Send(new EditProductName.Command { Product = product }));
    }

    [HttpDelete("{id}")] //api/GetProductDetail
    public async Task<IActionResult> DeleteProductName(int id)
    {
        return HandleResult(await Mediator.Send(new DeleteProductName.Command { Id = id }));
    }

}
