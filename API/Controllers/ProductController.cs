
using Application.History;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class ProductController : BaseApiController
    {
        [Authorize]
        [HttpPost]  //api/AddProduct
        public async Task<IActionResult> AddProduct(ProductDto product)
        {
            return HandleResult(await Mediator.Send(new AddProduct.AddProductCommand { Product = product }));
        }

        [HttpGet] //api/GetProducts
        public async Task<IActionResult> GetProductList()
        {
            return HandleResult(await Mediator.Send(new ProductList.Query()));
        }

        [HttpGet("{id}")] //api/GetProductDetail
        public async Task<IActionResult> GetProductDetail(int id)
        {
            return HandleResult(await Mediator.Send(new ProductDetail.Query { Id = id }));
        }
        [Authorize]
        [HttpPut] //api/UpdateProduct
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updatedProduct)
        {
            return HandleResult(await Mediator.Send(new EditProduct.Command { UpdatedProduct = updatedProduct }));
        }

        [HttpDelete("{id}")] //api/DeleteProduct//
        public async Task<IActionResult> DeleteProduct(string productName, int id)
        {
            return HandleResult(await Mediator.Send(new DeleteProduct.Command { Id = id }));
        }
    }
}
