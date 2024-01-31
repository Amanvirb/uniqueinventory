using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;
public class ProductController : BaseApiController
{

    //[Authorize(Roles = "SuperAdmin")]
    //[AllowAnonymous]
    //[HttpPost]
    //public async Task<IActionResult> AddProduct(ProductDto product)
    //{
    //    return HandleResult(await Mediator.Send(new AddProduct.AddProductCommand { Product = product }));
    //}

    //[Authorize(Roles = "SuperAdmin")]
    [AllowAnonymous]
    [HttpPost]
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
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductDetail(int id)
    {
        return HandleResult(await Mediator.Send(new ProductDetail.Query { Id = id }));
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpPut]
    public async Task<IActionResult> UpdateProduct(UpdateProductDto updatedProduct)
    {
        return HandleResult(await Mediator.Send(new EditProduct.Command { UpdatedProduct = updatedProduct }));
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        return HandleResult(await Mediator.Send(new DeleteProduct.Command { Id = id }));
    }
}
