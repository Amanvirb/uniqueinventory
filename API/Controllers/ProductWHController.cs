using Application.ProductNames.Dtoæ;
using Application.ProductNames;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;
public class ProductWHController : BaseApiController
{

    //[Authorize(Roles = "SuperAdmin")]
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductDetailDto product)
    {
        return HandleResult(await Mediator.Send(new AddProduct.AddProductCommand { Product = product }));
    }

    //[Authorize(Roles = "SuperAdmin,Admin,Employee")]
    [AllowAnonymous]
    [HttpPost("productname")]
    public async Task<IActionResult> CreateProductName(AddProductNameDto name)
    {
        return HandleResult(await Mediator.Send(new AddProductName.Command { Product = name }));
    }

    //[Authorize(Roles = "SuperAdmin")]
    [AllowAnonymous]
    [HttpPost("scanproducts")]
    public async Task<IActionResult> AddProducts(ScannedQRDto product)
    {
        return HandleResult(await Mediator.Send(new AddProducts.AddProductsCommand { Product = product }));
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetProductList()
    {
        return HandleResult(await Mediator.Send(new ProductList.Query()));
    }

    [AllowAnonymous]
    [HttpGet("{serialNo}")]
    public async Task<IActionResult> GetProductDetail(string serialNo)
    {
        return HandleResult(await Mediator.Send(new ProductDetail.Query { SerialNo = serialNo }));
    }

    //[Authorize(Roles = "SuperAdmin")]
    [AllowAnonymous]
    [HttpDelete("{serialNo}")]
    public async Task<IActionResult> DeleteProduct(string serialNo)
    {
        return HandleResult(await Mediator.Send(new DeleteProduct.Command { SerialNo = serialNo }));
    }

    //[Authorize(Roles = "SuperAdmin,Admin,Employee")]
    [AllowAnonymous]
    [HttpPut]
    public async Task<IActionResult> UpdateProductNameDetail(AddProductNameDto product)
    {
        return HandleResult(await Mediator.Send(new EditProductName.Command { Product = product }));
    }

    //[Authorize(Roles = "SuperAdmin,Admin,Employee")]
    //[AllowAnonymous]
    //[HttpDelete("productname/{id}")]
    //public async Task<IActionResult> DeleteProductName(int id)
    //{
    //    return HandleResult(await Mediator.Send(new DeleteProductName.Command { Id = id }));
    //}
}
