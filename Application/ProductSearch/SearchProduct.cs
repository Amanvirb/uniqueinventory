using Application.Extensions;

namespace Application.ProductSearch;
public class SearchProduct
{
    public class Query : IRequest<Result<List<ProductSearchDto>>>
    {
        public ProductSearchParams Params { get; set; }
    }
    public class Handler(DataContext context) : IRequestHandler<Query, Result<List<ProductSearchDto>>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<List<ProductSearchDto>>> Handle(Query request, CancellationToken ct)
        {
            var param = request.Params;

            var output = new List<ProductSearchDto>();

            List<ProductSearchProductNameDto> searchedProducts = [];

            var query = _context.Products
                 .Include(x => x.Location)
                 .Include(x => x.ProductName)
                 .Search(param)
                 .AsQueryable();

            var products = await query.ToListAsync(ct);

            var productNames = products.Select(x => x.ProductName.Name).Distinct().ToList();

            foreach (var ProductName in productNames)
            {
                var selectedProducts = products.Where(x => x.ProductName.Name == ProductName).ToList();

                var selectedLocations = selectedProducts.Select(x => x.Location.Name).Distinct().ToList();
               
                List<ProductSearchProductNameDto> searchedProductsName = [];

                foreach (var location in selectedLocations)
                {
                    var serials = selectedProducts
                        .Where(x => x.Location.Name == location)
                        .Select((p) => new ConsolidateSerialDto
                        {
                            SerialNo = p.SerialNumber
                        })
                        .ToList();

                    searchedProductsName.Add(new()
                    {
                        Location = location,
                        Quantity = serials.Count(),
                        SerialNumbers = serials,
                    });
                }

                output.Add(new()
                {
                    ProductName = ProductName,
                    Products = searchedProductsName
                });
            }

            return Result<List<ProductSearchDto>>.Success(output);
        }
    }
}