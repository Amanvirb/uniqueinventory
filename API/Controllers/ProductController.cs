
using Application.History;

namespace API.Controllers
{
    public class ProductController : BaseApiController
    {
        [HttpPost]  //api/AddProduct
        public async Task<IActionResult> AddProduct(ProductDto product)
        {
            return HandleResult(await Mediator.Send(new AddProduct.Command { Product = product }));
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

        [HttpGet("productHistory")]  //api/GetProductUpdateHistoryList
        public async Task<IActionResult> GetProductUpdateHistory()
        {
            return HandleResult(await Mediator.Send(new GetProductUpdateHistory.Query()));
        }

        [HttpPut] //api/UpdateProduct
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updatedProduct)
        {
            return HandleResult(await Mediator.Send(new EditProduct.Command { UpdatedProduct = updatedProduct }));
        }

    }
}
