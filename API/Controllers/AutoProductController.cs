using Microsoft.AspNetCore.Identity;
using Persistence.Seeds;

namespace API.Controllers
{
    public class AutoProductController : BaseApiController
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AutoProductController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost("AddRoles")] 
        public async Task<IActionResult> AddRoles()
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                await _roleManager.CreateAsync(new IdentityRole(Roles.Employee.ToString()));
                await _roleManager.CreateAsync(new IdentityRole(Roles.Customer.ToString()));
            }
            return Ok();
        }

        [HttpPost("AddProducts")]  //api/AddProducts
        public async Task<IActionResult> AddProducts(List<ProductDto> products)
        {
            foreach (var product in products)
            {
                var res = await Mediator.Send(new AddProduct.AddProductCommand { Product = product });
                if (!res.IsSuccess) return BadRequest(res.Error);

            }
            return Ok();
        }

        [HttpGet] //api/GenerateBulkProducts
        public List<ProductDto> GenerateProduct()
        {
            return GenerateAutoProduct();

        }

        [HttpPost("AddProductsAuto")]  //api/AddProducts
        public async Task<IActionResult> AddProductsAuto()
        {
            var products = GenerateAutoProduct();
            foreach (var product in products)
            {
                var res = await Mediator.Send(new AddProduct.AddProductCommand { Product = product });
                if (!res.IsSuccess) return BadRequest(res.Error);

            }
            return Ok();
        }

        private List<ProductDto> GenerateAutoProduct()
        {
            Random rnd = new();

            var output = new List<ProductDto>();
            for (int i = 1; i < 501; i++)
            {
                output.Add(new()
                {
                    SerialNumber = "SerialNo" + i.ToString(),
                    ProductNumberName = "ProductNumber" + rnd.Next(1, 250).ToString(),
                    LocationName = "Location" + rnd.Next(1, 100).ToString()
                });
            }
            return output;
        }
    }
}
